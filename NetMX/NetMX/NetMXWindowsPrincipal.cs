using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Collections.ObjectModel;

namespace NetMX
{
    public class NetMXWindowsPrincipal : WindowsPrincipal, INetMXPrincipal
    {
        private ReadOnlyCollection<MBeanPermission> _permissions;

		 public NetMXWindowsPrincipal(ReadOnlyCollection<MBeanPermission> permissions, WindowsIdentity identity)
            : base(identity)
        {
			  _permissions = permissions;
        }

        #region INetMXPrincipal Members
        public IEnumerable<MBeanPermission> Permissions
        {
            get { return _permissions; }
        }
        #endregion
    }
}
