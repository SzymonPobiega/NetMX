using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET;
using WSMan.NET.Eventing;

namespace NetMX.Remote.Jsr262.Server
{
   public class EventingRequestHandler : IEventingRequestHandler<NotificationResult>
   {      
      public EventingRequestHandler(IMBeanServer server)
      {
         _server = server;
      }
      
      public void Bind(IEventingRequestHandlerContext context, EndpointAddressBuilder susbcriptionManagerEndpointAddress)
      {
         ObjectName target = context.Selectors.ExtractObjectName();
         int listenerId = GenerateNextListenerId();
         susbcriptionManagerEndpointAddress.Headers.Add(new NotificationListenerListHeader(listenerId.ToString()));
         SubscriptionInfo subscriptionInfo = new SubscriptionInfo(context, listenerId);
         lock (_subscriptions)
         {
            _subscriptions.Add(subscriptionInfo);
         }
         _server.AddNotificationListener(target, subscriptionInfo.OnNotification, subscriptionInfo.FilterNotification, listenerId);
      }

      public void Unbind(IEventingRequestHandlerContext context)
      {            
         ObjectName target = context.Selectors.ExtractObjectName();        
         lock (_subscriptions)
         {
            SubscriptionInfo toRemove = _subscriptions.Single(x => x.EventingContext == context);
            _server.RemoveNotificationListener(target, toRemove.OnNotification, toRemove.FilterNotification, toRemove.ListenerId);
            _subscriptions.Remove(toRemove);            
         }        
      }

      private int GenerateNextListenerId()
      {
         lock (_synchRoot)
         {
            _lastListenerId++;
            return _lastListenerId;
         }
      }

      private readonly IMBeanServer _server;
      private int _lastListenerId;
      private readonly object _synchRoot = new object();
      private readonly List<SubscriptionInfo> _subscriptions = new List<SubscriptionInfo>();
   }
}