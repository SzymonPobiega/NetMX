using System;

namespace NetMX.Remote
{    
    public interface INetMXConnectorServerFactory
    {
        INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server);
    }
}
