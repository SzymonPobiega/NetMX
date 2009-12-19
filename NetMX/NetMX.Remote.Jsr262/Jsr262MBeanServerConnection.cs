using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Simon.WsManagement;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262
{
   internal sealed class Jsr262MBeanServerConnection : IMBeanServerConnection, IDisposable
   {
      private readonly ProxyFactory _proxyFactory;
      private readonly ManagementClient _manClient;
      private bool _disposed;

      public Jsr262MBeanServerConnection(ProxyFactory proxyFactory, ManagementClient managementClient)
      {
         _proxyFactory = proxyFactory;
         _manClient = managementClient;
      }

      #region IMBeanServerConnection Members
      public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         throw new NotImplementedException();
      }

      public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         throw new NotImplementedException();
      }

      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
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
                                                         ResourceEPR = new EndpointReferenceType(name)
                                                      };
         ObjectName objectName = _manClient.Create(Schema.MBeanServerResourceUri, request).ExtractObjectName();
         return new ObjectInstance(objectName, null);         
      }

      public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         throw new NotImplementedException();
      }

      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
      {
         throw new NotImplementedException();
      }

      public void RemoveNotificationListener(ObjectName name, ObjectName listener)
      {
         throw new NotImplementedException();
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

         _manClient.Put<DynamicMBeanResource>(Schema.DynamicMBeanResourceUri, null, request, ObjectNameSelector.CreateSelectorSet(name));
      }

      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         DynamicMBeanResource request = new DynamicMBeanResource
                                           {
                                              Property = namesAndValues.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray()
                                           };

         return _manClient.Put<DynamicMBeanResource>(Schema.DynamicMBeanResourceUri, null, request, ObjectNameSelector.CreateSelectorSet(name))
            .Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();         
      }

      public object GetAttribute(ObjectName name, string attributeName)
      {
         return _manClient.Get<DynamicMBeanResource>(Schema.DynamicMBeanResourceUri,
            new GetAttributesFragment(new[] { attributeName }).GetExpression(), null)
            .Property.First(x => x.name == attributeName).Deserialize();         
      }

      public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
      {
         return _manClient.Get<DynamicMBeanResource>(Schema.DynamicMBeanResourceUri,
            new GetAttributesFragment(attributeNames).GetExpression(), null)
            .Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();         
      }

      public int GetMBeanCount()
      {
         using (IDisposableProxy proxy = _proxyFactory.Create(null, Schema.DynamicMBeanResourceUri))
         {
            OperationContext.Current.OutgoingMessageHeaders.Add(new RequestTotalItemsTotalCountEstimate());
            proxy.Enumerate(null);
            TotalItemsTotalCountEstimate countEstimate =
               TotalItemsTotalCountEstimate.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
            return countEstimate.Value;
         }
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
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {            
//            XmlEnumerateResponse response = proxy.Enumerate(new Enumerate(true, EnumerationMode.EnumerateEPR, null, null));
            EnumerateResponseMessage response = proxy.Enumerate(null);
            return response.EnumerateResponse.DeserializeAsEPRs().Count() > 0;
         }
      }

      public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
      {
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
//            XmlEnumerateResponse response = proxy.Enumerate(new Enumerate(true, EnumerationMode.EnumerateEPR, Schema.QueryNamesDialect, null));
            EnumerateResponseMessage response = proxy.Enumerate(null);
            return response.EnumerateResponse.DeserializeAsEPRs().Select(x => x.ExtractObjectName());
         }
      }

      public void UnregisterMBean(ObjectName name)
      {
         _manClient.Delete(Schema.DynamicMBeanResourceUri, ObjectNameSelector.CreateSelectorSet(name));         
      }

      public string GetDefaultDomain()
      {
         return _manClient.Get<GetDefaultDomainResponse>(Schema.DynamicMBeanResourceUri,
                                                         IJsr262ServiceContractConstants.
                                                            GetDefaultDomainFragmentTransferPath).DomainName;         
      }

      public IList<string> GetDomains()
      {
         return _manClient.Get<GetDomainsResponse>(Schema.DynamicMBeanResourceUri,
                                                         IJsr262ServiceContractConstants.
                                                            GetDefaultDomainFragmentTransferPath).DomainNames.ToList();                  
      }
      #endregion

      public void Dispose()
      {
         if (!_disposed)
         {
            _proxyFactory.Dispose();
            //_manClient.Di
            _disposed = true;
         }
      }
   }
}
