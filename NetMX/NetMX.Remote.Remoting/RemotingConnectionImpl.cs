#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.Security.Principal;
#endregion

namespace NetMX.Remote.Remoting
{
	internal sealed class RemotingConnectionImpl : MarshalByRefObject, IRemotingConnection, IDisposable
	{
		#region MEMBERS
		private bool _disposed;
		private IMBeanServer _server;
		private RemotingServerImpl _remotingServer;
		private string _connectionId;
		private object _subject;
		private string _secutityProvider;
		private int _currentListenerId;
		private NotificationBuffer _buffer;
		private Dictionary<int, ListenerProxy> _listenerProxys;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnectionImpl(IMBeanServer server, RemotingServerImpl remotingServer, string connectionId, object subject, RemotingConnectionImplConfig config)
		{
			_server = server;
			_remotingServer = remotingServer;
			_connectionId = connectionId;
			_subject = subject;
			_secutityProvider = config.SecurityProvider;
			_buffer = new NotificationBuffer(config.BufferSize);
			_listenerProxys = new Dictionary<int, ListenerProxy>();
		}
		#endregion		

		#region Utility
		private int GetNextListenerId()
		{
			_currentListenerId++;				
			return _currentListenerId;			
		}
		private IPrincipal Authorize(object token)
		{
			return NetMXSecurityService.Authorize(_secutityProvider, _subject, token);
		}
		#endregion

		#region IRemotingConnection Members
		public object Invoke(object token, ObjectName name, string operationName, object[] arguments)
		{
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            return _server.Invoke(name, operationName, arguments);
         }
		}
		public void SetAttribute(object token, ObjectName name, string attributeName, object value)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				_server.SetAttribute(name, attributeName, value);
			}
		}
		public object GetAttribute(object token, ObjectName name, string attributeName)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.GetAttribute(name, attributeName);
			}
		}
		public IList<AttributeValue> GetAttributes(object token, ObjectName name, string[] attributeNames)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.GetAttributes(name, attributeNames);
			}
		}
		public MBeanInfo GetMBeanInfo(object token, ObjectName name)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.GetMBeanInfo(name);
			}
		}
		public void Close()
		{
			RemotingServices.Disconnect(this);
		}
		public string ConnectionId
		{
			get { return _connectionId; }
		}
		public int AddNotificationListener(object token, ObjectName name, NotificationFilterCallback filterCallback)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				int listenerId = GetNextListenerId();
				ListenerProxy proxy = new ListenerProxy(name, _buffer, listenerId, filterCallback);
				_listenerProxys.Add(listenerId, proxy);				
				_server.AddNotificationListener(name, proxy.NotificationCallback, filterCallback, listenerId);
				return listenerId;
			}
		}
		public void RemoveNotificationListener(object token, ObjectName name, int listenerId)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				ListenerProxy proxy = _listenerProxys[listenerId];
				if (proxy.HasFilterCallback)
				{
					_server.RemoveNotificationListener(name, proxy.NotificationCallback, proxy.NotificationFilterCallback, listenerId);
				}
				else
				{
					_server.RemoveNotificationListener(name, proxy.NotificationCallback, null, listenerId);
				}
			}
		}		
		public bool IsInstanceOf(object token, ObjectName name, string className)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.IsInstanceOf(name, className);
			}
		}
		public bool IsRegistered(object token, ObjectName name)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.IsRegistered(name);
			}
		}
		public void UnregisterMBean(object token, ObjectName name)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				_server.UnregisterMBean(name);
			}
		}
		public NotificationResult FetchNotifications(int startSequenceId, int maxCount)
		{
			return _buffer.FetchNotifications(startSequenceId, maxCount);
		}
		public IEnumerable<ObjectName> QueryNames(object token, ObjectName name, QueryExp query)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				return _server.QueryNames(name, query);
			}
		}
      public int GetMBeanCount(object token)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            return _server.GetMBeanCount();
         }
      }
      public void AddNotificationListener(object token, ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            _server.AddNotificationListener(name, listener, filterCallback, handback);
         }
      }

      public void RemoveNotificationListener(object token, ObjectName name, ObjectName listener, NotificationFilterCallback filterCallback, object handback)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            _server.RemoveNotificationListener(name, listener, filterCallback, handback);
         }
      }

      public void RemoveNotificationListener(object token, ObjectName name, ObjectName listener)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            _server.RemoveNotificationListener(name, listener);
         }
      }

      public IList<AttributeValue> SetAttributes(object token, ObjectName name, IEnumerable<AttributeValue> namesAndValues)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            return _server.SetAttributes(name, namesAndValues);
         }
      }

      public string GetDefaultDomain(object token)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            return _server.GetDefaultDomain();
         }
      }

      public IList<string> GetDomains(object token)
      {
         using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
         {
            return _server.GetDomains();
         }
      }
		#endregion

		#region IDisposable Members
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Close();
					_remotingServer.UnregisterConnection(this);
					foreach (ListenerProxy proxy in _listenerProxys.Values)
					{
						if (proxy.HasFilterCallback)
						{
							_server.RemoveNotificationListener(proxy.Name, proxy.NotificationCallback, proxy.NotificationFilterCallback, proxy.ListenerId);
						}
						else
						{
							_server.RemoveNotificationListener(proxy.Name, proxy.NotificationCallback, null, proxy.ListenerId);
						}
					}					
				}
				_disposed = true;
			}
		}
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion

		#region Security context
		private class TemporarySecurityContext : IDisposable
		{
			private IPrincipal _normal;

			public TemporarySecurityContext(IPrincipal temporary)
			{
				if (temporary != null)
				{
					_normal = Thread.CurrentPrincipal;
					Thread.CurrentPrincipal = temporary;
				}
			}
			
			public void Dispose()
			{
				if (_normal != null)
				{
					Thread.CurrentPrincipal = _normal;
				}
			}
		}
		#endregion

		#region Listener proxy
		private class ListenerProxy
		{
			private ObjectName _name;			
			private NotificationBuffer _buffer;
			private int _listenerId;			
			private NotificationFilterCallback _callback;

			public bool HasFilterCallback
			{
				get { return _callback != null; }
			}
			public ObjectName Name
			{
				get { return _name; }
			}
			public int ListenerId
			{
				get { return _listenerId; }
			}

			public ListenerProxy(ObjectName name, NotificationBuffer buffer, int listenerId, NotificationFilterCallback filterCallback)
			{
				_name = name;
				_buffer = buffer;
				_listenerId = listenerId;
			}

			public void NotificationCallback(Notification notification, object handback)
			{
				_buffer.AddNotification(new TargetedNotification(notification, _listenerId));
			}

			public bool NotificationFilterCallback(Notification notification)
			{
				return _callback(notification);
			}
		}
		#endregion							      
   }
}
