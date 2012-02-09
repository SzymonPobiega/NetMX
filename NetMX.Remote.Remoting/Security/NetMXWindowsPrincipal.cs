using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Collections.ObjectModel;

namespace NetMX.Remote.Remoting.Security
{
   internal class NetMXWindowsPrincipal : WindowsPrincipal, INetMXPrincipal
   {
      #region Members
      private ReadOnlyCollection<MBeanPermission> _permissions;
      #endregion

      #region Constructors
      public NetMXWindowsPrincipal(ReadOnlyCollection<MBeanPermission> permissions, WindowsIdentity identity)
         : base(identity)
      {
         _permissions = permissions;
      }
      #endregion

      #region INetMXPrincipal Members
      public IEnumerable<MBeanPermission> Permissions
      {
         get { return _permissions; }
      }
      #endregion
   }
}
