#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using NetMX.Remote.Remoting.Internal;
using System.Configuration;
#endregion

namespace NetMX.Remote.Remoting
{
	/// <summary>
	/// An implementation of <see cref="INetMXConnectorFactory"/> using .NET Remoting.
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
	public sealed class RemotingConnectorFactory : INetMXConnectorFactory
	{
		private readonly NotificationFetcherConfig _fetcherConfig;
		
		public INetMXConnector Connect(Uri serviceUrl, object credentials)
		{
			var connector = new RemotingConnector(serviceUrl, _fetcherConfig);
		    connector.Connect(credentials);
		    return connector;
		}

	    public RemotingConnectorFactory(NotificationFetcherConfig fetcherConfig)
	    {
	        const string channelName = "remotingConnectorClient";

	        _fetcherConfig = fetcherConfig;
            if (System.Runtime.Remoting.Channels.ChannelServices.GetChannel(channelName) == null)
            {
                IDictionary props = new Hashtable();
                props["name"] = channelName;
                props["secure"] = "true";
                props["tokenImpersonationLevel"] = "impersonation";
                var sinkProvier = new System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider();
                var tcc = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel(props, sinkProvier);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcc, false);
            }
	    }
	}
}
