#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Simon.Configuration;
using Simon.Configuration.Provider;
#endregion

namespace NetMX.Remote
{    
    [ConfigurationSection("netMXSecurityService")]
    public sealed class NetMXSecurityService : ServiceBase<NetMXSecurityProvider>
    {
        private static readonly NetMXSecurityService _instance = new NetMXSecurityService();

        public static object Authenticate(string provider, object credentials)
        {
            return _instance[provider].Authenticate(credentials);
        }
        public static INetMXPrincipal Authorize(string provider, object subject)
        {
            return _instance[provider].Authorize(subject);
        }        
    }
}
