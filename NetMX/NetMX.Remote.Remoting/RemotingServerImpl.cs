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
        private string _securityProvider;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingServerImpl(IMBeanServer server, string securityProvider)
		{
			_server = server;
            _securityProvider = securityProvider;
		}
		#endregion

		#region OVERRIDDEN		
		public override object InitializeLifetimeService()
		{
			return null;
		}
		#endregion

		#region IRemotingServer Members
		public IRemotingConnection NewClient(object credentials)
		{
            object subject = NetMXSecurityService.Authenticate(_securityProvider, credentials);
			RemotingConnectionImpl connection = new RemotingConnectionImpl(_server, subject, _securityProvider);			
			return connection;
		}
		#endregion
	}
}
