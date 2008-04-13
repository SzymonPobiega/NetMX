#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
#endregion

namespace NetMX.Remote.Remoting
{
	public sealed class RemotingServerProvider : NetMXConnectorServerProvider
	{
		#region MEMBERS
		private RemotingConnectionImplConfig _connectionConfig;
		#endregion

		#region OVERRIDDEN
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config, ConfigurationElement nestedElement)
		{
			base.Initialize(name, config, nestedElement);
			_connectionConfig = new RemotingConnectionImplConfig();			
			if (!string.IsNullOrEmpty(config["securityProvider"]))
			{
				_connectionConfig.SecurityProvider = config["securityProvider"];				
			}
			else
			{
				throw new ProviderException("Security provider is not specified.");
			}
			if (!string.IsNullOrEmpty(config["notificationBufferSize"]))
			{
				_connectionConfig.BufferSize = int.Parse(config["notificationBufferSize"]);
			}
		}
		public override INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
		{			
			return new RemotingConnectorServer(serviceUrl, server, _connectionConfig);
		}
		#endregion
	}
}
