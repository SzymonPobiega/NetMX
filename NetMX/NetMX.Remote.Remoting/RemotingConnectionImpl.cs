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
	internal sealed class RemotingConnectionImpl : MarshalByRefObject, IRemotingConnection
	{
		#region MEMBERS
		private IMBeanServer _server;
		private object _subject;
		private string _secutityProvider;
		private int _currentListenerId;
		private NotificationBuffer _buffer;
		private Dictionary<int, ListenerProxy> _listenerProxys;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnectionImpl(IMBeanServer server, object subject, RemotingConnectionImplConfig config)
		{
			_server = server;
			_subject = subject;
			_secutityProvider = config.SecurityProvider;
			_buffer = new NotificationBuffer(config.BufferSize);
			_listenerProxys = new Dictionary<int, ListenerProxy>();
		}
		#endregion		

		#region Utility
		private int GetNextListenerId()
		{
			return _currentListenerId;
			_currentListenerId++;				
		}
		private IPrincipal Authorize(object token)
		{
			return NetMXSecurityService.Authorize(_secutityProvider, _subject, token);
		}
		#endregion

		#region IRemotingConnection Members
		public object Invoke(object token, ObjectName name, string operationName, object[] arguments)
		{
			IPrincipal p = Thread.CurrentPrincipal;
			try
			{
				Thread.CurrentPrincipal = NetMXSecurityService.Authorize(_secutityProvider, _subject, token);
				return _server.Invoke(name, operationName, arguments);
			}
			finally
			{
				Thread.CurrentPrincipal = p;
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
		public int AddNotificationListener(object token, ObjectName name, NotificationFilterCallback filterCallback)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				int listenerId = GetNextListenerId();
				ListenerProxy proxy = new ListenerProxy(_buffer, listenerId, filterCallback);
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
		public void unregisterMBean(object token, ObjectName name)
		{
			using (TemporarySecurityContext tsc = new TemporarySecurityContext(Authorize(token)))
			{
				_server.UnregisterMBean(name);
			}
		}
		#endregion

		#region Security context
		private class TemporarySecurityContext : IDisposable
		{
			private IPrincipal _normal;

			public TemporarySecurityContext(IPrincipal temporary)
			{
				_normal = Thread.CurrentPrincipal;
				Thread.CurrentPrincipal = temporary;
			}
			
			public void Dispose()
			{
				Thread.CurrentPrincipal = _normal;
			}
		}
		#endregion

		#region Listener proxy
		private class ListenerProxy
		{
			private NotificationBuffer _buffer;
			private int _listenerId;
			private NotificationFilterCallback _callback;

			public bool HasFilterCallback
			{
				get { return _callback != null; }
			}

			public ListenerProxy(NotificationBuffer buffer, int listenerId, NotificationFilterCallback filterCallback)
			{
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
