#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using NetMX.Remote.Remoting.Internal;
#endregion

namespace NetMX.Remote.Remoting
{
	[Serializable]
	internal sealed class RemotingConnector : INetMXConnector,  ISerializable
	{
		#region MEMBERS
		private bool _disposed;
		private Uri _serviceUrl;
		private NotificationFetcherConfig _fetcherConfig;
		private IRemotingConnection _connection;
		private RemotingMBeanServerConnection _serverConnection;
		private NotificationFetcher _fetcher;
		private object _token;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public RemotingConnector(Uri serviceUrl, NotificationFetcherConfig fetcherConfig)
		{
			_serviceUrl = serviceUrl;
			_fetcherConfig = fetcherConfig;
		}
		private RemotingConnector(SerializationInfo info, StreamingContext context)			
		{
			string connectionId = info.GetString("connectionId");
			_serviceUrl = new Uri(info.GetString("serviceUrl"));
			string typeString = info.GetString("tokenType");			
			if (typeString != null)
			{
				Type tokenType = Type.GetType(typeString, true);
				_token = info.GetValue("token", tokenType);
			}
			_fetcherConfig = (NotificationFetcherConfig) info.GetValue("fetcherConfig", typeof(NotificationFetcherConfig));
			if (connectionId != null)
			{
				IRemotingServer server = (IRemotingServer)Activator.GetObject(typeof(IRemotingServer), _serviceUrl.ToString());
				_connection = server.Reconnect(connectionId);
				_serverConnection = new RemotingMBeanServerConnection(_connection, _token);
				_fetcher = new NotificationFetcher(_fetcherConfig, _connection, _serverConnection);
			}
		}		
		#endregion

		#region INetMXConnector Members
		public void Close()
		{
			if (_connection != null)
			{
				if (_fetcher != null)
				{
					_fetcher.Dispose();
					_fetcher = null;
				}
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
			_token = token;
			_serverConnection = new RemotingMBeanServerConnection(_connection, _token);
			_fetcher = new NotificationFetcher(_fetcherConfig, _connection, _serverConnection);
		}

		public string ConnectionId
		{
			get { return _connection.ConnectionId; }
		}

		public IMBeanServerConnection MBeanServerConnection
		{
			get { return _serverConnection; }
		}
		#endregion

		#region IDisposable Members
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_fetcher.Dispose();
				}
				_disposed = true;
			}
		}
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion

		#region ISerializable Members
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (_connection != null)
			{
				info.AddValue("connectionId", _connection.ConnectionId);
			}
			info.AddValue("serviceUrl", _serviceUrl);
			info.AddValue("fetcherConfig", _fetcherConfig);
			if (_token != null)
			{
				info.AddValue("tokenType", _token.GetType().AssemblyQualifiedName);
				info.AddValue("token", _token);
			}
		}
		#endregion
	}
}
