#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
#endregion

namespace NetMX.Remote.Remoting
{
	public sealed class RemotingConnectorProvider : NetMXConnectorProvider
	{
		#region MEMBERS
		#endregion
		
		#region OVERRIDDEN        
		public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
		{
			return new RemotingConnector(serviceUrl);
		}
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);

			IDictionary props = new Hashtable();
			props["name"] = "remotingConnectorClient";
			props["secure"] = "true";
			props["tokenImpersonationLevel"] = "impersonation";
			System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider sinkProvier = new System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider();
			System.Runtime.Remoting.Channels.Tcp.TcpClientChannel tcc = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, sinkProvier);
			System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcc, true);		 
		}
		#endregion
	}
}
