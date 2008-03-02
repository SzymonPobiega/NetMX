#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote.Remoting
{
	internal class RemotingConnector : INetMXConnector
	{
		#region MEMBERS
		private bool _disposed;
		private Uri _serviceUrl;
		private IRemotingConnection _connection;
		private IMBeanServerConnection _serverConnection;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnector(Uri serviceUrl)
		{
			_serviceUrl = serviceUrl;
		}
		#endregion

		#region INetMXConnector Members
		public void Close()
		{
			if (_connection != null)
			{
				_serverConnection = null;
				_connection.Close();
				_connection = null;
			}
		}

		public void Connect(object credentials)
		{
			object token;
			IRemotingServer server = (IRemotingServer)Activator.GetObject(typeof(IRemotingServer), _serviceUrl.ToString());
			_connection = server.NewClient(credentials, out token);
			_serverConnection = new RemotingMBeanServerConnection(_connection, token);
		}

		public string ConnectionId
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public IMBeanServerConnection MBeanServerConnection
		{
			get { return _serverConnection; }
		}
		#endregion

		#region IDisposable Members
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Close();
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
