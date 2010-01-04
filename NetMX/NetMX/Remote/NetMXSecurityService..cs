#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX.Remote
{    
    [ConfigurationSection("netMXSecurityService")]
    public sealed class NetMXSecurityService : ServiceBase<NetMXSecurityProvider>
    {
        private static readonly NetMXSecurityService _instance = new NetMXSecurityService();

        public static void Authenticate(string provider, object credentials, out object subject, out object token)
        {
            _instance[provider].Authenticate(credentials, out subject, out token);
        }
        public static INetMXPrincipal Authorize(string provider, object subject, object token)
        {
            return _instance[provider].Authorize(subject, token);
        }        
    }
}
