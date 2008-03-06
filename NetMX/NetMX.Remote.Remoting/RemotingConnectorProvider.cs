#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using NetMX.Remote.Remoting.Internal;
#endregion

namespace NetMX.Remote.Remoting
{
	/// <summary>
	/// An implementation of <see cref="NetMX.Remote.NetMXConnectorProvider"/> using .NET Remoting.
	/// </summary>
	/// <remarks>
	/// Configuration properties:
	/// <list type="bullet">
	/// <item>notificationFetchPolicy: Proactive, OnReconnect - policy of handling notifications. 
	/// Proactive policy means creation of fetcher thread for each connector. OnReconnect policy means that
	/// pending notifications will only be fetched when connector is deserialized (which means reconnection
	/// to server).</item>
	/// </list>
	/// </remarks>
	public sealed class RemotingConnectorProvider : NetMXConnectorProvider
	{
		#region MEMBERS
		private NotificationFetcherConfig _fetcherConfig;
		#endregion
		
		#region OVERRIDDEN        
		public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
		{
			return new RemotingConnector(serviceUrl, _fetcherConfig);
		}
		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);

			_fetcherConfig = new NotificationFetcherConfig(config);

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
