using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote.Remoting
{
	public interface IRemotingConnection
	{
		int AddNotificationListener(object token, ObjectName name, NotificationFilterCallback filterCallback);
      void AddNotificationListener(object token, ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback);
		void RemoveNotificationListener(object token, ObjectName name, int listenerId);
      void RemoveNotificationListener(object token, ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback);
      void RemoveNotificationListener(object token, ObjectName name, ObjectName listener);      

		object Invoke(object token, ObjectName name, string operationName, object[] arguments);
		void SetAttribute(object token, ObjectName name, string attributeName, object value);
      IList<AttributeValue> SetAttributes(object token, ObjectName name, IEnumerable<AttributeValue> namesAndValues);
		object GetAttribute(object token, ObjectName name, string attributeName);
		IList<AttributeValue> GetAttributes(object token, ObjectName name, string[] attributeNames);

      int GetMBeanCount(object token);
		MBeanInfo GetMBeanInfo(object token, ObjectName name);
		bool IsInstanceOf(object token, ObjectName name, string className);
		bool IsRegistered(object token, ObjectName name);
		void UnregisterMBean(object token, ObjectName name);		
		IEnumerable<ObjectName> QueryNames(object token, ObjectName name, QueryExp query);            

      string GetDefaultDomain(object token);
      IList<string> GetDomains(object token);

      void Close();
      string ConnectionId { get; }
      NotificationResult FetchNotifications(int startSequenceId, int maxCount);
   }
}
