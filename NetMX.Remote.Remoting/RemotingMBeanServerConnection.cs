#region USING
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
      private Dictionary<NotificationCallback, List<int>> _listenerGroupProxys = new Dictionary<NotificationCallback, List<int>>();
		private Dictionary<int, NotificationSubscription> _reverseListenerProxys = new Dictionary<int, NotificationSubscription>();
		#endregion

		#region CONSTRUCTOR
		public RemotingMBeanServerConnection(IRemotingConnection connection, object token)
		{
			_connection = connection;
			_token = token;
		}
		#endregion

		public void Notify(TargetedNotification targetedNotification)
		{
			NotificationSubscription subsr;
			if (_reverseListenerProxys.TryGetValue(targetedNotification.ListenerId, out subsr))
			{
				subsr.Callback(targetedNotification.Notification, subsr.Handback);
			}
		}

		#region IMBeanServerConnection Members
      public ObjectInstance CreateMBean(string className, ObjectName name, object[] arguments)
      {
         return _connection.CreateMBean(_token, className, name, arguments);
      }
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
			_reverseListenerProxys[listenerId] = subscr;
         List<int> listenerGroup;
         if (_listenerGroupProxys.TryGetValue(callback, out listenerGroup))
         {
            listenerGroup.Add(listenerId);
         }
         else
         {
            listenerGroup = new List<int>();
            listenerGroup.Add(listenerId);
            _listenerGroupProxys[callback] = listenerGroup;
         }
		}
		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
		{
			int listenerId;
         NotificationSubscription key = new NotificationSubscription(callback, filterCallback, handback);
			if (_listenerProxys.TryGetValue(key, out listenerId))
			{            
				_connection.RemoveNotificationListener(_token, name, listenerId);

				_reverseListenerProxys.Remove(listenerId);
            _listenerProxys.Remove(key);

            List<int> listenerGroup = _listenerGroupProxys[callback];
            listenerGroup.Remove(listenerId);
            if (listenerGroup.Count == 0)
            {
               _listenerGroupProxys.Remove(callback);
            }
			}
			else
			{
				throw new ListenerNotFoundException(name.ToString());
			}
		}
		public void RemoveNotificationListener(ObjectName name, NotificationCallback callback)
		{
         List<int> listenerGroup;
         if (_listenerGroupProxys.TryGetValue(callback, out listenerGroup))
         {
            foreach (int listenerId in listenerGroup)
            {
               _connection.RemoveNotificationListener(_token, name, listenerId);

               NotificationSubscription key = _reverseListenerProxys[listenerId];
               _reverseListenerProxys.Remove(listenerId);
               _listenerProxys.Remove(key);
            }
            _listenerGroupProxys.Remove(callback);
         }
         else
         {
            throw new ListenerNotFoundException(name.ToString());
         }
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
			_connection.UnregisterMBean(_token, name);
		}		
      public int GetMBeanCount()
      {
         return _connection.GetMBeanCount(_token);
      }
      public IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query)
		{
			return _connection.QueryNames(_token, name, query);
		}		
      public void AddNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         _connection.AddNotificationListener(_token, name, listener, filterCallback, handback);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         _connection.RemoveNotificationListener(_token, name, listener, filterCallback, handback);
      }
      public void RemoveNotificationListener(ObjectName name, ObjectName listener)
      {
         _connection.RemoveNotificationListener(_token, name, listener);
      }
      public IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         return _connection.SetAttributes(_token, name, namesAndValues);
      }
      public string GetDefaultDomain()
      {
         return _connection.GetDefaultDomain(_token);
      }
      public IList<string> GetDomains()
      {
         return _connection.GetDomains(_token);
      }
      #endregion
   }
}
