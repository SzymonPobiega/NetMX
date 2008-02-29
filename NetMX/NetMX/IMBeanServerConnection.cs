#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	public interface IMBeanServerConnection
	{
        void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
        void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
        void RemoveNotificationListener(ObjectName name, NotificationCallback callback);

		object Invoke(ObjectName name, string operationName, object[] arguments);

		void SetAttribute(ObjectName name, string attributeName, object value);
		object GetAttribute(ObjectName name, string attributeName);

		MBeanInfo GetMBeanInfo(ObjectName name);
	}
}
