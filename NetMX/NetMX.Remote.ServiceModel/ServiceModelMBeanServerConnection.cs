using System;
using System.Collections.Generic;

namespace NetMX.Remote.ServiceModel
{
   internal sealed class ServiceModelMBeanServerConnection : IMBeanServerConnection
   {
      private readonly IMBeanServerContract _proxy;

      public ServiceModelMBeanServerConnection(IMBeanServerContract proxy)
      {
         _proxy = proxy;
      }

      #region IMBeanServerConnection Members
      public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         throw new InvalidOperationException("This operation is not supported by ServiceModel connector.");
      }
      public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         throw new InvalidOperationException("This operation is not supported by ServiceModel connector.");
      }
      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         throw new InvalidOperationException("This operation is not supported by ServiceModel connector.");
      }
      public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
      {
         return _proxy.CreateMBean(className, name, arguments);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         throw new InvalidOperationException("This operation is not supported by ServiceModel connector.");
      }
      public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
      {
         throw new InvalidOperationException("This operation is not supported by ServiceModel connector.");
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener)
      {
         _proxy.RemoveNotificationListener(name, listener);
      }
      public object Invoke(ObjectName name, string operationName, object[] arguments)
      {
         return _proxy.Invoke(name, operationName, arguments);
      }
      public void SetAttribute(ObjectName name, string attributeName, object value)
      {
         _proxy.SetAttribute(name, attributeName, value);
      }
      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         return _proxy.SetAttributes(name, namesAndValues);
      }
      public object GetAttribute(ObjectName name, string attributeName)
      {
         return _proxy.GetAttribute(name, attributeName);
      }
      public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
      {
         return _proxy.GetAttributes(name, attributeNames);
      }
      public int GetMBeanCount()
      {
         return _proxy.GetMBeanCount();
      }
      public MBeanInfo GetMBeanInfo(ObjectName name)
      {
         return _proxy.GetMBeanInfo(name);
      }
      public bool IsInstanceOf(ObjectName name, string className)
      {
         return _proxy.IsInstanceOf(name, className);
      }
      public bool IsRegistered(ObjectName name)
      {
         return _proxy.IsRegistered(name);
      }
      public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
      {
         return _proxy.QueryNames(name, query);
      }
      public void UnregisterMBean(ObjectName name)
      {
         _proxy.UnregisterMBean(name);
      }
      public string GetDefaultDomain()
      {
         return _proxy.GetDefaultDomain();
      }
      public IList<string> GetDomains()
      {
         return _proxy.GetDomains();
      }
      #endregion
   }
}
