using System;

namespace NetMX.Remote
{
    public interface INetMXConnectorFactory
    {
        INetMXConnector Connect(Uri serviceUrl, object credentials);
    }    
}
