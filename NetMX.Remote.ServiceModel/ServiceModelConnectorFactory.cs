using System;

namespace NetMX.Remote.ServiceModel
{
    public sealed class ServiceModelConnectorFactory : INetMXConnectorFactory
    {
        private readonly string _configurationName;

        public ServiceModelConnectorFactory(string configurationName)
        {
            _configurationName = configurationName;
        }


        public INetMXConnector Connect(Uri serviceUrl, object credentials)
        {
            var connector = new ServiceModelConnector(_configurationName, serviceUrl);
            connector.Connect(credentials);
            return connector;
        }
    }
}
