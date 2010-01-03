#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;
using Simon.Configuration;

#endregion

namespace NetMX.Remote
{    
    [ConfigurationSection("netMXConnectorServerFactory")]
    public sealed class NetMXConnectorServerFactory : ServiceBase<NetMXConnectorServerProvider>
    {
        private static readonly NetMXConnectorServerFactory _instance = new NetMXConnectorServerFactory();
        
        public static INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
        {            
            return _instance[serviceUrl.Scheme].NewNetMXConnectorServer(serviceUrl, server);
        }        
    }
}
