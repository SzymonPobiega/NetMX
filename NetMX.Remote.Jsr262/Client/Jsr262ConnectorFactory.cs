using System;

namespace NetMX.Remote.Jsr262
{
    public sealed class Jsr262ConnectorFactory : INetMXConnectorFactory
    {
        private readonly int _enumerationMaxElements;

        public Jsr262ConnectorFactory()
            : this(1500)
        {
        }

        public Jsr262ConnectorFactory(int enumerationMaxElements)
        {
            _enumerationMaxElements = enumerationMaxElements;
        }

        public INetMXConnector Connect(Uri serviceUrl, object credentials)
        {
            var connector = new Jsr262Connector(serviceUrl.ToString(), _enumerationMaxElements);
            connector.Connect(credentials);
            return connector;
        }
    }
}
