#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
#endregion

namespace NetMX
{
    public interface INetMXPrincipal : IPrincipal
    {
        IEnumerable<MBeanPermission> Permissions { get; }
    }
}
