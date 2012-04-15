using System;

namespace NetMX.Remote.HttpAdaptor
{
    internal sealed class HttpConnectorServer : INetMXConnectorServer
    {
        private readonly string _serviceUrl;
        private readonly IMBeanServer _server;

        public HttpConnectorServer(string serviceUrl, IMBeanServer server)
        {
            _serviceUrl = serviceUrl;
            _server = server;
        }

        public void Dispose()
        {
            
        }

        public IMBeanServer MBeanServer
        {
            get { return _server; }
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }

    }
}
