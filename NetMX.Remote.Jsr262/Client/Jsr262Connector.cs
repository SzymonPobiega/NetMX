using System;
using NetMX.Remote.Jsr262.Client;


namespace NetMX.Remote.Jsr262
{
    public sealed class Jsr262Connector : INetMXConnector
    {
        private readonly string _serviceUrl;
        private readonly int _enumerationMaxElements;
        private Jsr262MBeanServerConnection _connection;
        private bool _disposed;
        private Guid _connectionId;

        public Jsr262Connector(string serviceUrl, int enumerationMaxElements)
        {
            _serviceUrl = serviceUrl;
            _enumerationMaxElements = enumerationMaxElements;
        }

        public void Close()
        {
            _connection.Dispose();
            _connection = null;
        }
        public void Connect(object credentials)
        {
            _connectionId = Guid.NewGuid();
            _connection = new Jsr262MBeanServerConnection(_enumerationMaxElements, _serviceUrl);
        }
        public string ConnectionId
        {
            get { return _connectionId.ToString(); }
        }
        public IMBeanServerConnection MBeanServerConnection
        {
            get
            {
                return _connection;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            try
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
