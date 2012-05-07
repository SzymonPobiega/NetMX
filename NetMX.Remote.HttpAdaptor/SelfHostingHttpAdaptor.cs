using System;
using System.Web.Http.SelfHost;

namespace NetMX.Remote.HttpAdaptor
{
    public class SelfHostingHttpAdaptor : HttpAdaptor
    {
        private HttpSelfHostServer _server;
        private readonly string _listenAddress;
        private readonly IMBeanServerConnection _serverConnection;

        public SelfHostingHttpAdaptor(IMBeanServerConnection serverConnection, string listenAddress)
        {
            _listenAddress = listenAddress;
            _serverConnection = serverConnection;
        }

        public void Start()
        {
            if (_server != null)
            {
                throw new InvalidOperationException("Server is already started.");
            }
            var config = new HttpSelfHostConfiguration(_listenAddress);
            Configure(config, _serverConnection, _listenAddress);
            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
        }

        public void Stop()
        {
            if (_server == null)
            {
                throw new InvalidOperationException("Server is already stopped.");
            }
            _server.CloseAsync().Wait();
        }
    }
}