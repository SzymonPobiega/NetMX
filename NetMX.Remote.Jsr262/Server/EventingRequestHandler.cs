using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Eventing.Server;
using WSMan.NET.Management;
using WSMan.NET.SOAP;

namespace NetMX.Remote.Jsr262.Server
{
    public class EventingRequestHandler : IEventingRequestHandler<NotificationResult>
    {
        private readonly IMBeanServer _server;
        private int _lastListenerId;
        private readonly object _synchRoot = new object();
        private readonly List<SubscriptionInfo> _subscriptions = new List<SubscriptionInfo>();

        public EventingRequestHandler(IMBeanServer server)
        {
            _server = server;
        }

        public IDisposable Subscribe(IEventSink eventSink, object filterInstance, EndpointReference subscriptionManagerReference, IIncomingHeaders headers)
        {
            var selectorSetHeader = headers.GetHeader<SelectorSetHeader>();
            var target = selectorSetHeader.Selectors.ExtractObjectName();
            var listenerId = GenerateNextListenerId();
            subscriptionManagerReference.AddProperty(new NotificationListenerListHeader(listenerId.ToString()),false);
            var subscriptionInfo = new SubscriptionInfo(eventSink, listenerId);
            lock (_subscriptions)
            {
                _subscriptions.Add(subscriptionInfo);
            }
            _server.AddNotificationListener(target, subscriptionInfo.OnNotification, subscriptionInfo.FilterNotification, listenerId);

            return new SubscriptionRemover(
                () =>
                    {
                        lock (_subscriptions)
                        {
                            var toRemove = _subscriptions.Single(x => x.EventSink == eventSink);
                            _server.RemoveNotificationListener(target, toRemove.OnNotification, toRemove.FilterNotification, toRemove.ListenerId);
                            _subscriptions.Remove(toRemove);
                        }
                    });
        }

        private int GenerateNextListenerId()
        {
            lock (_synchRoot)
            {
                _lastListenerId++;
                return _lastListenerId;
            }
        }

        private class SubscriptionRemover : IDisposable
        {
            private bool _disposed;
            private readonly Action _unsubscribeAction;

            public SubscriptionRemover(Action unsubscribeAction)
            {
                _unsubscribeAction = unsubscribeAction;
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }
                _unsubscribeAction();
                _disposed = true;
            }
        }
    }
}