using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote.Remoting
{
	public interface IRemotingConnection
	{
		int AddNotificationListener(object token, ObjectName name, NotificationFilterCallback filterCallback);
		void RemoveNotificationListener(object token, ObjectName name, int listenerId);		

		object Invoke(object token, ObjectName name, string operationName, object[] arguments);
		void SetAttribute(object token, ObjectName name, string attributeName, object value);
		object GetAttribute(object token, ObjectName name, string attributeName);
		IList<AttributeValue> GetAttributes(object token, ObjectName name, string[] attributeNames);
		MBeanInfo GetMBeanInfo(object token, ObjectName name);

		bool IsInstanceOf(object token, ObjectName name, string className);
		bool IsRegistered(object token, ObjectName name);
		void unregisterMBean(object token, ObjectName name);

		void Close();
	}
}
