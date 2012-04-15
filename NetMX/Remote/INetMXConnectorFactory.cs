#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX.Configuration.Provider;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
    public interface INetMXConnectorFactory
    {
        INetMXConnector Connect(Uri serviceUrl, object credentials);
    }    
}
