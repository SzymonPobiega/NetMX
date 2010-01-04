using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Configuration.Provider;
using Simon.Configuration;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
	public class WindowsSecurityProvider : NetMXSecurityProvider
	{
		#region MEMBERS
		private Dictionary<SecurityIdentifier, IEnumerable<MBeanPermission>> _permissionMap;
		#endregion

		#region OVERRIDDEN
		public override void Authenticate(object credentials, out object subject, out object token)
		{
			subject = null;
			token = null;
		}
		public override INetMXPrincipal Authorize(object subject, object token)
		{
			List<MBeanPermission> permList = new List<MBeanPermission>();			
			WindowsPrincipal princ = (WindowsPrincipal)Thread.CurrentPrincipal;
			foreach (SecurityIdentifier role in _permissionMap.Keys)
			{
				if (princ.IsInRole(role))
				{
					permList.AddRange(_permissionMap[role]);
				}
			}
			return new NetMXWindowsPrincipal(permList.AsReadOnly(), (WindowsIdentity)Thread.CurrentPrincipal.Identity);
		}
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config, ConfigurationElement nestedElement)
		{
			base.Initialize(name, config, nestedElement);			
			RoleCollection roles = (RoleCollection)nestedElement;			
			_permissionMap = new Dictionary<SecurityIdentifier, IEnumerable<MBeanPermission>>();
			foreach (RoleElement role in roles)
			{
			   List<MBeanPermission> permissionList = new List<MBeanPermission>();
			   foreach (PermissionElement perm in role.Permissions)
			   {
			      permissionList.Add(new MBeanPermission(perm.Pattern, perm.Actions));
			   }
			   NTAccount identity = new NTAccount(role.Name);            
            try
            {
               SecurityIdentifier si = (SecurityIdentifier)identity.Translate(typeof(SecurityIdentifier));
               _permissionMap[si] = permissionList;
            }
            catch (IdentityNotMappedException ex)
            {
               Trace.WriteLine(string.Format("Invalid identity specification: {0}", role.Name));
            }
			}
		}
		#endregion
	}
}
