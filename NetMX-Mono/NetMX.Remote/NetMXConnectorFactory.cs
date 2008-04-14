#region USING
using System;
using System.Collections.Generic;
using System.Text;
using Simon.Configuration.Provider;
using Simon.Configuration;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
    [ConfigurationSection("netMXConnectorFactory")]
    public sealed class NetMXConnectorFactory : ServiceBase<NetMXConnectorProvider>
    {
        private static readonly NetMXConnectorFactory _instance = new NetMXConnectorFactory();

        public static INetMXConnector NewNetMXConnector(Uri serviceUrl)
        {
            return _instance[serviceUrl.Scheme].NewNetMXConnector(serviceUrl);
        }

        public static INetMXConnector Connect(Uri serviceUrl, object credentials)
        {
            INetMXConnector connector = NewNetMXConnector(serviceUrl);
            connector.Connect(credentials);
            return connector;
        }
    }    
}