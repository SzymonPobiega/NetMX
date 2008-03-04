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
		private object _token;
		private Dictionary<NotificationSubscription, int> _listenerProxys = new Dictionary<NotificationSubscription, int>();
		#endregion

		#region CONSTRUCTOR
		public RemotingMBeanServerConnection(IRemotingConnection connection, object token)
		{
			_connection = connection;
			_token = token;
		}
		#endregion

		#region IMBeanServerConnection Members
		public object Invoke(ObjectName name, string operationName, object[] arguments)
		{
			return _connection.Invoke(_token, name, operationName, arguments);
		}

		public void SetAttribute(ObjectName name, string attributeName, object value)
		{
			_connection.SetAttribute(_token, name, attributeName, value);
		}
		public object GetAttribute(ObjectName name, string attributeName)
		{
			return _connection.GetAttribute(_token, name, attributeName);
		}
		public IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames)
		{
			return _connection.GetAttributes(_token, name, attributeNames);
		}
		public MBeanInfo GetMBeanInfo(ObjectName name)
		{
			return _connection.GetMBeanInfo(_token, name);
		}
		public void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{			
			int listenerId = _connection.AddNotificationListener(_token, name, filterCallback);
			NotificationSubscription subscr = new NotificationSubscription(callback, filterCallback, handback);
			_listenerProxys[subscr] = listenerId;
		}
		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{
			int listenerId;
			if (_listenerProxys.TryGetValue(new NotificationSubscription(callback, filterCallback, handback), out listenerId))
			{
				_connection.RemoveNotificationListener(_token, name, listenerId);
			}
			else
			{
				throw new ListenerNotFoundException(name.ToString());
			}
		}
		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
		{
			throw new Exception("The method or operation is not implemented.");
		}		
		public bool IsInstanceOf(ObjectName name, string className)
		{
			return _connection.IsInstanceOf(_token, name, className);
		}
		public bool IsRegistered(ObjectName name)
		{
			return _connection.IsRegistered(_token, name);
		}
		public void UnregisterMBean(ObjectName name)
		{
			_connection.unregisterMBean(_token, name);
		}
		#endregion

		#region Listener proxy
		private class ClientListenerProxy
		{
			private int _listenerId;
			private object _handback;
			private NotificationCallback _callback;

			public ClientListenerProxy(int listenerId, NotificationCallback callback, object handback)
			{
				_listenerId = listenerId;
				_callback = callback;
				_handback = handback;
			}

		}
		#endregion		
	}
}
