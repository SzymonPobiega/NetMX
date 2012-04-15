#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX.Remote
{
    public interface INetMXSecurityProvider
    {
        void Authenticate(object credentials, out object subject, out object token);
        INetMXPrincipal Authorize(object subject, object token);
    }
}
