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
         throw new NotImplementedException();
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
         throw new NotImplementedException();
      }

      public void SetAttribute(ObjectName name, string attributeName, object value)
      {
         throw new NotImplementedException();
      }

      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         throw new NotImplementedException();
      }

      public object GetAttribute(ObjectName name, string attributeName)
      {         
         FragmentTransferHeader fragmentTransferHeader = new FragmentTransferHeader(
            new GetAttributesFragment(new [] {attributeName}).GetExpression());

         DynamicMBeanResource beanResource;
         using (IDisposableProxy proxy = _proxyFactory.Create(name, @"http://jsr262.dev.java.net/DynamicMBeanResource"))
         {            
            OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);
            beanResource = proxy.GetAttributes();            
         }

         return beanResource.Property.First(x => x.name == attributeName).Deserialize();
      }

      public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
      {
         throw new NotImplementedException();
      }

      public int GetMBeanCount()
      {
         throw new NotImplementedException();
      }

      public MBeanInfo GetMBeanInfo(ObjectName name)
      {
         throw new NotImplementedException();
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
