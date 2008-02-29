#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
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
		private TcpChannel _channel;
		private IMBeanServer _server;
		private RemotingServerImpl _remotingServer;
        private string _securityProvider;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnectorServer(Uri serviceUrl, IMBeanServer server, string securityProvider)
		{
			if (!serviceUrl.IsLoopback)
			{
				throw new ArgumentException("Cannot start server on remote host", "serviceUrl");
			}
			_serviceUrl = serviceUrl;
			_server = server;
            _securityProvider = securityProvider;
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
				_channel = new TcpChannel(port);				
				ChannelServices.RegisterChannel(_channel, true);
				_remotingServer = new RemotingServerImpl(_server, _securityProvider);
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
