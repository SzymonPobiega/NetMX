#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote.Remoting
{
	internal class RemotingMBeanServerConnection : IMBeanServerConnection
	{
		#region MEMBERS
		private IRemotingConnection _connection;
		#endregion		

		#region CONSTRUCTOR
		public RemotingMBeanServerConnection(IRemotingConnection connection)
		{
			_connection = connection;
		}
		#endregion

		#region IMBeanServerConnection Members
		public object Invoke(ObjectName name, string operationName, object[] arguments)
		{
			return _connection.Invoke(name, operationName, arguments);
		}

		public void SetAttribute(ObjectName name, string attributeName, object value)
		{
			_connection.SetAttribute(name, attributeName, value);
		}

		public object GetAttribute(ObjectName name, string attributeName)
		{
			return _connection.GetAttribute(name, attributeName);
		}

		public MBeanInfo GetMBeanInfo(ObjectName name)
		{
			return _connection.GetMBeanInfo(name);
		}		
        public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
