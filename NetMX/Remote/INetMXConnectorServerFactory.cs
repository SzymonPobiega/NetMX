#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX.Remote
{    
    public interface INetMXConnectorServerFactory
    {
        INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server);
    }
}
