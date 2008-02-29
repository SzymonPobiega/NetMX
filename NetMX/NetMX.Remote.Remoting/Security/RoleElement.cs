using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class RoleElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired=true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }
        [ConfigurationProperty("permissions", IsRequired = true)]
        public PermissionCollection Permissions
        {
            get { return (PermissionCollection)this["permissions"]; }
        }
    }
}
