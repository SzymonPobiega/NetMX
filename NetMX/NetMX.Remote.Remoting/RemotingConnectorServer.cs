#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Collections;
#endregion

namespace NetMX.Remote.Remoting
{
	internal class RemotingConnectorServer : INetMXConnectorServer
	{
		#region MEMBERS
		private bool _disposed;
		private bool _started;
		private bool _stopped;
		private Uri _serviceUrl;
		private TcpServerChannel _channel;
		private IMBeanServer _server;
		private RemotingServerImpl _remotingServer;
		private RemotingConnectionImplConfig _connectionConfig;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnectorServer(Uri serviceUrl, IMBeanServer server, RemotingConnectionImplConfig connectionConfig)
		{
			if (!serviceUrl.IsLoopback)
			{
				throw new ArgumentException("Cannot start server on remote host", "serviceUrl");
			}
			_serviceUrl = serviceUrl;
			_server = server;
			_connectionConfig = connectionConfig;
		}
		#endregion

		#region INetMXConnectorServer Members
		public IMBeanServer MBeanServer
		{
			get { return _server; }
		}
		public void Start()
		{
			if (_stopped)
			{
				throw new InvalidOperationException("Stopped server cannot be started again.");
			}
			if (!_started)
			{
				int port = _serviceUrl.Port;				
				BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider();
				IDictionary props = new Hashtable();
				props["name"] = "remotingConnector"+port;
				//props["secure"] = "true";
				props["port"] = port;
				//props["impersonate"] = "true";
				_channel = new TcpServerChannel(props, sinkProvider);
				ChannelServices.RegisterChannel(_channel, false);
				_remotingServer = new RemotingServerImpl(_server, _connectionConfig);
				RemotingServices.Marshal(_remotingServer, _serviceUrl.AbsolutePath.Trim('/'));
				_started = true;
			}
		}
		public void Stop()
		{
			if (!_stopped)
			{
				if (_channel != null)
				{
					ChannelServices.UnregisterChannel(_channel);
					RemotingServices.Disconnect(_remotingServer);
					_stopped = true;
				}
			}
		}
		#endregion

		#region IDisposable Members
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Stop();
				}
				_disposed = true;
			}
		}
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
