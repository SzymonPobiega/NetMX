#region USING
using System;

#endregion

namespace NetMX.Remote.Remoting
{
    public sealed class RemotingConnectorServerFactory : INetMXConnectorServerFactory
    {
        private readonly int _bufferSize;
        private readonly INetMXSecurityProvider _securityProvider;

        public RemotingConnectorServerFactory(int bufferSize, INetMXSecurityProvider securityProvider)
        {
            _bufferSize = bufferSize;
            _securityProvider = securityProvider;
        }

        public INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
		{			
			return new RemotingConnectorServer(serviceUrl, server, _securityProvider, _bufferSize);
		}
	}
}
