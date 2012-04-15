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
		private IMBeanServer _server;
	    private readonly INetMXSecurityProvider _securityProvider;
	    private readonly int _bufferSize;
	    private Dictionary<string, RemotingConnectionImpl> _connections = new Dictionary<string, RemotingConnectionImpl>();

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingServerImpl(IMBeanServer server, INetMXSecurityProvider securityProvider, int bufferSize)
		{
		    _server = server;
		    _securityProvider = securityProvider;
		    _bufferSize = bufferSize;
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
			_securityProvider.Authenticate(credentials, out subject, out token);
			string connectionId = Guid.NewGuid().ToString();
			var connection = new RemotingConnectionImpl(_server, _securityProvider, this, connectionId, subject, _bufferSize );
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
