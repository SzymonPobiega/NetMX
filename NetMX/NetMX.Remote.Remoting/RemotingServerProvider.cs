#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote.Remoting
{
	public sealed class RemotingServerProvider : NetMXConnectorServerProvider
	{
		#region MEMBERS
        private string _securityProvider;
		#endregion
		
		#region OVERRIDDEN		
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            _securityProvider = config["securityProvider"];
            if (string.IsNullOrEmpty(_securityProvider))
            {
                throw new ProviderException("Security provider is not specified.");
            }
        }
		public override INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
		{
			return new RemotingConnectorServer(serviceUrl, server, _securityProvider);
		}
		#endregion
	}
}
