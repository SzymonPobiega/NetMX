using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class PermissionElement : ConfigurationElement
    {
        [ConfigurationProperty("pattern", IsRequired=true)]
        public string Pattern
        {
            get { return (string)this["pattern"]; }
        }
        [ConfigurationProperty("actions", IsRequired = true)]
        public MBeanPermissionAction Actions
        {
            get { return (MBeanPermissionAction)this["actions"]; }
        }
    }
}
