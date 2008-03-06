#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
#endregion

namespace NetMX.Remote.Remoting
{
	internal class RemotingServerImpl : MarshalByRefObject, IRemotingServer
	{
		#region MEMBERS
		private IMBeanServer _server;
		private RemotingConnectionImplConfig _connectionConfig;
		private Dictionary<string, RemotingConnectionImpl> _connections = new Dictionary<string, RemotingConnectionImpl>();
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingServerImpl(IMBeanServer server, RemotingConnectionImplConfig connectionConfig)
		{
			_server = server;
			_connectionConfig = connectionConfig;
		}
		#endregion

		#region OVERRIDDEN
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

		#region INTERNAL INTERFACE
		internal void UnregisterConnection(RemotingConnectionImpl connection)
		{
			lock (_connections)
			{
				_connections.Remove(connection.ConnectionId);
			}
		}
		#endregion

		#region IRemotingServer Members
		public IRemotingConnection NewClient(object credentials, out object token)
		{
			object subject;
			NetMXSecurityService.Authenticate(_connectionConfig.SecurityProvider, credentials, out subject, out token);
			string connectionId = Guid.NewGuid().ToString();
			RemotingConnectionImpl connection = new RemotingConnectionImpl(_server, this, connectionId, subject, _connectionConfig);
			lock (_connections)
			{
				_connections.Add(connectionId, connection);
			}
			return connection;
		}
		public IRemotingConnection Reconnect(string connectionId)
		{
			RemotingConnectionImpl conn;
			if (_connections.TryGetValue(connectionId, out conn))
			{				
				return conn;
			}
			throw new ArgumentException("connectionId");
		}
		#endregion
	}
}
