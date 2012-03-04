using System;
using System.Collections.Generic;
using System.Linq;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET.Addressing.Faults;
using WSMan.NET.SOAP;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing.Client;
using WSMan.NET.Management;
using WSMan.NET.Server;

namespace NetMX.Remote.Jsr262.Client
{
    internal sealed class Jsr262MBeanServerConnection : IMBeanServerConnection, IDisposable
    {
        private readonly int _enumerationMaxElements;
        private readonly ManagementClient _manClient;
        private readonly EnumerationClient _enumClient;
        private readonly EventingClient _eventingClient;
        private readonly SOAPClient _soapClient;
        private readonly Dictionary<NotificationSubscriptionKey, PullSubscriptionListener> _subscriptions = new Dictionary<NotificationSubscriptionKey, PullSubscriptionListener>();
        private bool _disposed;

        public Jsr262MBeanServerConnection(int enumerationMaxElements, string serverUri)
        {
            _enumerationMaxElements = enumerationMaxElements;
            _soapClient = new SOAPClient(serverUri);
            _manClient = new ManagementClient(serverUri);
            _enumClient = new EnumerationClient(true, serverUri);
            _eventingClient = new EventingClient(serverUri);
        }

        #region IMBeanServerConnection Members
        public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            var key = new NotificationSubscriptionKey(name, callback, filterCallback, handback);
            if (_subscriptions.ContainsKey(key))
            {
                throw new InvalidOperationException("Subscription already exists.");
            }
            var pullDeliverySubscription = _eventingClient.SubscribeUsingPullDelivery<TargetedNotificationType>(new Filter(Schema.NotificationDialect, null), new Mandatory(name.CreateSelectorSetHeader()));
            var listener = new PullSubscriptionListener(pullDeliverySubscription, callback, filterCallback, handback);
            _subscriptions.Add(key, listener);
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            var key = new NotificationSubscriptionKey(name, callback, filterCallback, handback);

            PullSubscriptionListener listener;
            if (_subscriptions.TryGetValue(key, out listener))
            {
                _subscriptions.Remove(key);
                listener.Dispose();
            }
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            var toRemove = _subscriptions.Keys.Where(x => callback.Equals(x.Callback) && name.Equals(x.ObjectName)).ToList();
            foreach (var key in toRemove)
            {
                var listener = _subscriptions[key];
                listener.Dispose();
                _subscriptions.Remove(key);
            }
        }


        public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
        {
            throw new NotImplementedException();
        }

        public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
        {
            throw new NotImplementedException();
        }

        public void RemoveNotificationListener(ObjectName name, ObjectName listener)
        {
            throw new NotImplementedException();
        }

        public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
        {
            var request = new DynamicMBeanResourceConstructor
                              {
                                  RegistrationParameters =
                                      arguments.Select(x => new ParameterType(null, x)).ToArray(),
                                  ResourceClass = className,
                                  ResourceEPR = ObjectNameSelector.CreateEndpointAddress(name)
                              };
            var objectName = _manClient.Create(Schema.MBeanServerResourceUri, request).ExtractObjectName();
            return new ObjectInstance(objectName, null);
        }

        public object Invoke(ObjectName name, string operationName, object[] arguments)
        {
            var request = new OperationRequestType
                              {
                                  Input = arguments.Select(x => new ParameterType(null, x)).ToArray(),
                                  name = operationName,
                                  Signature = null
                              };

            var responseMessage = _soapClient.BuildMessage()
                .WithAction(Schema.InvokeAction)
                .WithSelectors(name.CreateSelectorSet())
                .WithResourceUri(Schema.DynamicMBeanResourceUri)
                .AddBody(new InvokeMessage(request))
                .SendAndGetResponse();

            var payload = responseMessage.GetPayload<InvokeResponseMessage>();
            return payload.ManagedResourceOperationResult.Deserialize();
        }

        public void SetAttribute(ObjectName name, string attributeName, object value)
        {
            var request = new DynamicMBeanResource
                              {
                                  Property = new[]
                                               {
                                                   new NamedGenericValueType(attributeName, value),
                                               }
                              };

            _manClient.Put<XmlFragment<DynamicMBeanResource>>(Schema.DynamicMBeanResourceUri, new GetAttributesFragment(attributeName).GetExpression(), new XmlFragment<DynamicMBeanResource>(request), name.CreateSelectorSet());
        }

        public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
        {
            var request = new DynamicMBeanResource
                              {
                                  Property =
                                      namesAndValues.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray()
                              };
            var names = namesAndValues.Select(x => x.Name);

            return _manClient.Put<XmlFragment<DynamicMBeanResource>>(
                Schema.DynamicMBeanResourceUri,
                new GetAttributesFragment(names).GetExpression(),
                new XmlFragment<DynamicMBeanResource>(request), name.CreateSelectorSet())
               .Value.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
        }

        public object GetAttribute(ObjectName name, string attributeName)
        {
            try
            {
                return _manClient.Get<XmlFragment<DynamicMBeanResource>>(Schema.DynamicMBeanResourceUri,
                                                                      new GetAttributesFragment(attributeName).GetExpression(), name.CreateSelectorSet())
                .Value.Property.First(x => x.name == attributeName).Deserialize();
            }
            catch (FaultException ex)
            {
                if (new EndpointUnavailableFaultException().Equals(ex))
                {
                    throw new InstanceNotFoundException(name);
                }
                throw;
            }
        }

        public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
        {
            return _manClient.Get<XmlFragment<DynamicMBeanResource>>(Schema.DynamicMBeanResourceUri,
                                                                     new GetAttributesFragment(attributeNames).GetExpression(), null)
               .Value.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
        }

        public int GetMBeanCount()
        {
            return _enumClient.EstimateCount(Schema.DynamicMBeanResourceUri, null);
        }

        public MBeanInfo GetMBeanInfo(ObjectName name)
        {
            var responseMessage = _soapClient.BuildMessage()
                .WithAction(Schema.GetMBeanInfoAction)
                .WithSelectors(name.CreateSelectorSet())
                .WithResourceUri(Schema.DynamicMBeanResourceUri)
                .SendAndGetResponse();

            var payload = responseMessage.GetPayload<ResourceMetaDataTypeMessage>();
            return payload.DynamicMBeanResourceMetaData.Deserialize();
        }

        public bool IsInstanceOf(ObjectName name, string className)
        {
            var responseMessage = _soapClient.BuildMessage()
                .WithAction(Schema.InstanceOfAction)
                .WithSelectors(name.CreateSelectorSet())
                .WithResourceUri(Schema.DynamicMBeanResourceUri)
                .AddBody(new IsInstanceOfMessage(className))
                .SendAndGetResponse();

            var payload = responseMessage.GetPayload<IsInstanceOfResponseMessage>();
            return payload.Value;
        }

        public bool IsRegistered(ObjectName name)
        {
            return _enumClient.EnumerateEPR(Schema.DynamicMBeanResourceUri, null, 1,
                                            name.CreateSelectorSet()).Count() > 0;
        }

        public IEnumerable<ObjectName> QueryNames(ObjectName name, IExpression<bool> query)
        {
            var filter = new Filter(Schema.QueryNamesDialect, query);
            return _enumClient.EnumerateEPR(Schema.DynamicMBeanResourceUri, filter, _enumerationMaxElements,
                                            name.CreateSelectorSet())
               .Select(x => x.ExtractObjectName());
        }

        public void UnregisterMBean(ObjectName name)
        {
            _manClient.Delete(Schema.DynamicMBeanResourceUri, name.CreateSelectorSet());
        }

        public string GetDefaultDomain()
        {
            return _manClient.Get<XmlFragment<GetDefaultDomainResponse>>(Schema.MBeanServerResourceUri,
                                                            IJsr262ServiceContractConstants.
                                                               GetDefaultDomainFragmentTransferPath).Value.DomainName;
        }

        public IList<string> GetDomains()
        {
            return _manClient.Get<XmlFragment<GetDomainsResponse>>(Schema.MBeanServerResourceUri,
                                                      IJsr262ServiceContractConstants.
                                                         GetDomainsFragmentTransferPath).Value.DomainNames.ToList();
        }
        #endregion

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            foreach (PullSubscriptionListener listener in _subscriptions.Values)
            {
                listener.Dispose();
            }
            _disposed = true;
        }

        

    }
}