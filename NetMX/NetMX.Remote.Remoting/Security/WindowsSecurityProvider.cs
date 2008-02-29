using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Configuration.Provider;
using Simon.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class WindowsSecurityProvider : NetMXSecurityProvider
    {
        #region MEMBERS
        private Dictionary<SecurityIdentifier, IEnumerable<MBeanPermission>> _permissionMap;
        #endregion

        #region OVERRIDDEN
        public override object Authenticate(object credentials)
        {
            return null;
        }
        public override INetMXPrincipal Authorize(object subject)
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
            return new NetMXWindowsPrincipal(permList, (WindowsIdentity)Thread.CurrentPrincipal.Identity); 
        }
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            string configSectionName = config["roleMap"];
            if (string.IsNullOrEmpty(configSectionName))
            {
                throw new ProviderException(string.Format("Configuration section not specified for WindowsSecurityProvider {0}.",name));
            }
            WindowsSecurityProviderConfigurationSection section = TypedConfigurationManager.GetSection<WindowsSecurityProviderConfigurationSection>(configSectionName, true);
            _permissionMap = new Dictionary<SecurityIdentifier, IEnumerable<MBeanPermission>>();
            foreach (RoleElement role in section.Roles)
            {
                List<MBeanPermission> permissionList = new List<MBeanPermission>();
                foreach (PermissionElement perm in role.Permissions)
                {
                    permissionList.Add(new MBeanPermission(perm.Pattern, perm.Actions));
                }
                NTAccount identity = new NTAccount(role.Name);
                //IdentityReferenceCollection sourceAccounts = new IdentityReferenceCollection(1);
                //sourceAccounts.Add(identity);
                SecurityIdentifier si = (SecurityIdentifier)identity.Translate(typeof(SecurityIdentifier));
                //IdentityReferenceCollection references2 = NTAccount.Translate(sourceAccounts, typeof(SecurityIdentifier), false);                
                //_permissionMap[(SecurityIdentifier)references2[0]] = permissionList;
                _permissionMap[si] = permissionList;
            }
        }
        #endregion
    }
}
