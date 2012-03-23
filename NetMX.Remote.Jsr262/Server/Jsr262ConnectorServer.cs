using System;
using NetMX.Remote.Jsr262.Server;
using NetMX.Remote.Jsr262.Structures.Query;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing.Server;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262
{
    internal sealed class Jsr262ConnectorServer : INetMXConnectorServer
    {
        private readonly string _serviceUrl;
        private readonly IMBeanServer _server;
        private HttpListenerTransferEndpoint _serviceHost;

        public Jsr262ConnectorServer(string serviceUrl, IMBeanServer server)
        {
            _serviceUrl = serviceUrl;
            _server = server;
        }

        public void Dispose()
        {
            if (_serviceHost != null)
            {
                Stop();
            }
        }

        public IMBeanServer MBeanServer
        {
            get { return _server; }
        }

        public void Start()
        {
            if (_serviceHost != null)
            {
                throw new InvalidOperationException("Server is already started.");
            }

            var managementHandler = new ManagementTransferRequestHandler();
            managementHandler.Bind(Schema.DynamicMBeanResourceUri, new DynamicMBeanManagementRequestHandler(_server));
            managementHandler.Bind(Schema.MBeanServerResourceUri, new MBeanServerManagementRequestHandler(_server));

            var enumerationServer = new EnumerationServer();
            enumerationServer.Bind(Schema.DynamicMBeanResourceUri, Schema.QueryNamesDialect, typeof(string), new QueryNamesEnumerationRequestHandler(_server));
            enumerationServer.Bind(Schema.DynamicMBeanResourceUri, FilterMap.DefaultDialect, typeof(void), new IsRegisteredEnumerationRequestHandler(_server));

            var eventingServer = new EventingServer(new EventingRequestHandler(_server));
            eventingServer.Bind(Schema.NotificationDialect, typeof (NotificationFilter));
            eventingServer.EnablePullDeliveryUsing(enumerationServer.PullServer);

            var extensionHandler = new Jsr262ExtensionMethodHandler(_server);

            _serviceHost = new HttpListenerTransferEndpoint(_serviceUrl,
                new TransferServer(managementHandler),
                enumerationServer,
                enumerationServer,
                eventingServer,
                extensionHandler);
        }

        public void Stop()
        {
            if (_serviceHost == null)
            {
                throw new InvalidOperationException("Server is already stopped.");
            }
            _serviceHost.Dispose();
            _serviceHost = null;
        }

    }
}
