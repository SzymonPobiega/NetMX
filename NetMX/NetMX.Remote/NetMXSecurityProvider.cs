#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
    public abstract class NetMXSecurityProvider : ProviderBase
    {
        public abstract object Authenticate(object credentials);
        public abstract INetMXPrincipal Authorize(object subject);
    }
}
