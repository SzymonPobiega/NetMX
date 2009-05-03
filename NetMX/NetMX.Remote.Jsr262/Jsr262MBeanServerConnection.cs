﻿using System;
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
            return proxy.Invoke(request).Deserialize();
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
            proxy.SetAttributes(request);
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
            DynamicMBeanResource response = proxy.SetAttributes(request);
            return response.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
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
            beanResource = proxy.GetAttributes();
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
            beanResource = proxy.GetAttributes();
         }

         return beanResource.Property.Select(x => new AttributeValue(x.name, x.Deserialize())).ToList();
      }

      public int GetMBeanCount()
      {
         throw new NotImplementedException();
      }

      public MBeanInfo GetMBeanInfo(ObjectName name)
      {         
         using (IDisposableProxy proxy = _proxyFactory.Create(name, Schema.DynamicMBeanResourceUri))
         {
            return proxy.GetMBeanInfo().Deserialize();
         }
      }

      public bool IsInstanceOf(ObjectName name, string className)
      {
         throw new NotImplementedException();
      }

      public bool IsRegistered(ObjectName name)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
      {
         throw new NotImplementedException();
      }

      public void UnregisterMBean(ObjectName name)
      {
         throw new NotImplementedException();
      }

      public string GetDefaultDomain()
      {
         throw new NotImplementedException();
      }

      public IList<string> GetDomains()
      {
         throw new NotImplementedException();
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
