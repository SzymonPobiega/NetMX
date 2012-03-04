#region USING
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
#endregion

namespace NetMX.Remote.ServiceModel
{
   /// <summary>
   /// WCF service implementation for exposing <see cref="IMBeanServerConnection"/> instance as a service.
   /// </summary>
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class MBeanServerService : IMBeanServerContract
	{
      private readonly IMBeanServerConnection _exportedServer;

      public MBeanServerService(IMBeanServerConnection exportedServer)
      {
         _exportedServer = exportedServer;  
      }

      #region IMBeanServerConnection Members
      public void AddNotificationListener(ObjectName name, ObjectName listener)
      {
         _exportedServer.AddNotificationListener(name, listener, null, null);
      }
      public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
      {
         return _exportedServer.CreateMBean(className, name, arguments);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener)
      {
         _exportedServer.RemoveNotificationListener(name, listener);
      }
      public object Invoke(ObjectName name, string operationName, object[] arguments)
      {
         return _exportedServer.Invoke(name, operationName, arguments);
      }

      public void SetAttribute(ObjectName name, string attributeName, object value)
      {
         _exportedServer.SetAttribute(name, attributeName, value);
      }

      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         return _exportedServer.SetAttributes(name, namesAndValues);
      }

      public object GetAttribute(ObjectName name, string attributeName)
      {
         return _exportedServer.GetAttribute(name, attributeName);
      }

      public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
      {
         return _exportedServer.GetAttributes(name, attributeNames);
      }

      public int GetMBeanCount()
      {
         return _exportedServer.GetMBeanCount();         
      }

      public MBeanInfo GetMBeanInfo(ObjectName name)
      {
         return _exportedServer.GetMBeanInfo(name);
      }

      public bool IsInstanceOf(ObjectName name, string className)
      {
         return _exportedServer.IsInstanceOf(name, className);
      }

      public bool IsRegistered(ObjectName name)
      {
         return _exportedServer.IsRegistered(name);
      }

      public IEnumerable<ObjectName> QueryNames(ObjectName name, IExpression<bool> query)
      {
         return _exportedServer.QueryNames(name, query);
      }

      public void UnregisterMBean(ObjectName name)
      {
         _exportedServer.UnregisterMBean(name);
      }

      public string GetDefaultDomain()
      {
         return _exportedServer.GetDefaultDomain();
      }

      public IList<string> GetDomains()
      {
         return _exportedServer.GetDomains();
      }
      #endregion
   }
}
