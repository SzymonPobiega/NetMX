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

		#region IRemotingServer Members
		public IRemotingConnection NewClient(object credentials, out object token)
		{
			object subject;
			NetMXSecurityService.Authenticate(_connectionConfig.SecurityProvider, credentials, out subject, out token);
			RemotingConnectionImpl connection = new RemotingConnectionImpl(_server, subject, _connectionConfig);
			return connection;
		}
		#endregion
	}
}
