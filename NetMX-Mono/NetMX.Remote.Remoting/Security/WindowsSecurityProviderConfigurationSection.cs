using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class WindowsSecurityProviderConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("roles", IsRequired=true)]
        public RoleCollection Roles
        {
            get { return (RoleCollection)this["roles"]; }
        }
    }
}
