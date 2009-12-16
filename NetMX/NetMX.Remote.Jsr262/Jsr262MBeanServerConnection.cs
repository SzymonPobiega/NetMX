using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   internal sealed class Jsr262MBeanServerConnection : IMBeanServerConnection, IDisposable
   {
      private readonly ProxyFactory _proxyFactory;
      private bool _disposed;

      public Jsr262MBeanServerConnection(ProxyFactory proxyFactory)
      {
         _proxyFactory = proxyFactory;
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
         using (IDisposableProxy proxy = _proxyFactory.Create(null, Schema.MBeanServerResourceUri))
         {
            EndpointReferenceType response = proxy.CreateMBean(request);
            return new ObjectInstance(response.ObjectName, null);
         }
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

         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            //throw new NotImplementedException();
            proxy.SetAttributes(new SetAttributesMessage(request));
         }
      }

      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         DynamicMBeanResource request = new DynamicMBeanResource
                                           {
                                              Property = namesAndValues.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray()
                                           };

         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            //throw new NotImplementedException();
            SetAttributesResponseMessage response = proxy.SetAttributes(new SetAttributesMessage(request));
            return response.Response.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
         }         
      }

      public object GetAttribute(ObjectName name, string attributeName)
      {
         FragmentTransferHeader fragmentTransferHeader = new FragmentTransferHeader(
            new GetAttributesFragment(new[] { attributeName }).GetExpression());

         DynamicMBeanResource beanResource;
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);
            beanResource = (DynamicMBeanResource) proxy.Get().Response;
         }

         return beanResource.Property.First(x => x.name == attributeName).Deserialize();
      }

      public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
      {
         FragmentTransferHeader fragmentTransferHeader = new FragmentTransferHeader(
            new GetAttributesFragment(attributeNames).GetExpression());

         DynamicMBeanResource beanResource;
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);
            beanResource = (DynamicMBeanResource) proxy.Get().Response;
         }

         return beanResource.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
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
         try
         {
            using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
            {
               proxy.UnregisterMBean();
            }
         }
         catch (FaultException ex)
         {
            if (WsAddressing.IsEndpointUnavailable(ex))
            {
               throw new InstanceNotFoundException(name);
            }
            throw;
         }         
      }

      public string GetDefaultDomain()
      {
         FragmentTransferHeader fragmentTransferHeader = new FragmentTransferHeader(IJsr262ServiceContractConstants.GetDefaultDomainFragmentTransferPath);         
         using (IDisposableProxy proxy = _proxyFactory.Create(null, Schema.DynamicMBeanResourceUri))
         {
            OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);
            GetDefaultDomainResponse response = (GetDefaultDomainResponse)proxy.Get().Response;
            return response.DomainName;
         }         
      }

      public IList<string> GetDomains()
      {
         FragmentTransferHeader fragmentTransferHeader = new FragmentTransferHeader(IJsr262ServiceContractConstants.GetDomainsFragmentTransferPath);
         using (IDisposableProxy proxy = _proxyFactory.Create(null, Schema.DynamicMBeanResourceUri))
         {
            OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);
            GetDomainsResponse response = (GetDomainsResponse)proxy.Get().Response;
            return new List<string>(response.DomainNames);
         }         
      }
      #endregion

      public void Dispose()
      {
         if (!_disposed)
         {
            _proxyFactory.Dispose();
            _disposed = true;
         }
      }
   }
}
