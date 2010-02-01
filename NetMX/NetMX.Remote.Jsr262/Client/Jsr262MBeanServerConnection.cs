using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WSMan.NET;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262.Client
{
   internal sealed class Jsr262MBeanServerConnection : IMBeanServerConnection, IDisposable
   {
      public Jsr262MBeanServerConnection(ProxyFactory proxyFactory, ManagementClient managementClient, EnumerationClient enumerationClient, EventingClient eventingClient)
      {
         _proxyFactory = proxyFactory;
         _manClient = managementClient;
         _enumClient = enumerationClient;
         _eventingClient = eventingClient;
      }

      #region IMBeanServerConnection Members
      public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         NotificationSubscriptionKey key = new NotificationSubscriptionKey(name, callback, filterCallback, handback);
         if (_subscriptions.ContainsKey(key))
         {
            throw new InvalidOperationException("Subscription already exists.");
         }
         PullSubscriptionListener listener =
            new PullSubscriptionListener(
               _eventingClient.SubscribeUsingPullDelivery<NotificationResult>(new Uri(Schema.DynamicMBeanResourceUri),
                                                                             null, name.CreateSelectorSet()), callback,
               filterCallback, handback);
         _subscriptions.Add(key, listener);
      }

      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         NotificationSubscriptionKey key = new NotificationSubscriptionKey(name, callback, filterCallback, handback);

         PullSubscriptionListener listener;
         if (_subscriptions.TryGetValue(key, out listener))
         {
            _subscriptions.Remove(key);
            listener.Dispose();
         }
      }

      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
      {
         IList<NotificationSubscriptionKey> toRemove =
            _subscriptions.Keys.Where(x => ReferenceEquals(callback, x.Callback) && name.Equals(x.ObjectName)).ToList();
         foreach (NotificationSubscriptionKey key in toRemove)
         {
            PullSubscriptionListener listener = _subscriptions[key];
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
         DynamicMBeanResourceConstructor request = new DynamicMBeanResourceConstructor
                                                      {
                                                         RegistrationParameters =
                                                            arguments.Select(x => new ParameterType(null, x)).ToArray(),
                                                         ResourceClass = className,
                                                         ResourceEPR = new EndpointReference(ObjectNameSelector.CreateEndpointAddress(name))
                                                      };
         ObjectName objectName = _manClient.Create(Schema.MBeanServerResourceUri, request).ExtractObjectName();
         return new ObjectInstance(objectName, null);
      }      

      public object Invoke(ObjectName name, string operationName, object[] arguments)
      {
         OperationRequestType request = new OperationRequestType
                                           {
                                              Input = arguments.Select(x => new ParameterType(null, x)).ToArray(),
                                              name = operationName,
                                              Signature = null
                                           };
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            return proxy.Invoke(new InvokeMessage(request)).ManagedResourceOperationResult.Deserialize();
         }
      }

      public void SetAttribute(ObjectName name, string attributeName, object value)
      {
         DynamicMBeanResource request = new DynamicMBeanResource
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
         DynamicMBeanResource request = new DynamicMBeanResource
                                           {
                                              Property = namesAndValues.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray()
                                           };
         IEnumerable<string> names = namesAndValues.Select(x => x.Name);

         return _manClient.Put<XmlFragment<DynamicMBeanResource>>(Schema.DynamicMBeanResourceUri, new GetAttributesFragment(names).GetExpression(), new XmlFragment<DynamicMBeanResource>(request), name.CreateSelectorSet())
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
            if (ex.IsA(Faults.EndpointUnavailable))
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
         return _enumClient.EstimateCount(new Uri(Schema.DynamicMBeanResourceUri), null);         
      }

      public MBeanInfo GetMBeanInfo(ObjectName name)
      {
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            return proxy.GetMBeanInfo().DynamicMBeanResourceMetaData.Deserialize();
         }
      }

      public bool IsInstanceOf(ObjectName name, string className)
      {
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            return proxy.IsInstanceOf(new IsInstanceOfMessage(className)).Value;
         }
      }

      public bool IsRegistered(ObjectName name)
      {
         return _enumClient.EnumerateEPR(new Uri(Schema.DynamicMBeanResourceUri), null, 1,
                                         name.CreateSelectorSet()).Count() > 0;
      }

      public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
      {
         Filter filter = new Filter(Schema.QueryNamesDialect, query);
         return _enumClient.EnumerateEPR(new Uri(Schema.DynamicMBeanResourceUri), filter, 1500,
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

         _proxyFactory.Dispose();
         //_manClient.
         _enumClient.Dispose();
         if (_eventingClient != null)
         {
            _eventingClient.Dispose();
         }
         foreach (PullSubscriptionListener listener in _subscriptions.Values)
         {
            listener.Dispose();
         }

         _disposed = true;
      }

      private readonly ProxyFactory _proxyFactory;
      private readonly ManagementClient _manClient;
      private readonly EnumerationClient _enumClient;
      private readonly EventingClient _eventingClient;
      private readonly Dictionary<NotificationSubscriptionKey, PullSubscriptionListener> _subscriptions = new Dictionary<NotificationSubscriptionKey, PullSubscriptionListener>();
      private bool _disposed;

   }
}