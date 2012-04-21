using System;
using System.Web.Http.SelfHost;

namespace NetMX.Remote.HttpAdaptor
{
    public class SelfHostingHttpAdaptor : HttpAdaptor
    {
        private HttpSelfHostServer _server;
        private readonly string _listenAddress;

        public SelfHostingHttpAdaptor(string listenAddress)
        {
            _listenAddress = listenAddress;
        }

        public void Start()
        {
            if (_server != null)
            {
                throw new InvalidOperationException("Server is already started.");
            }
            var config = new HttpSelfHostConfiguration(_listenAddress);
            Configure(config);
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