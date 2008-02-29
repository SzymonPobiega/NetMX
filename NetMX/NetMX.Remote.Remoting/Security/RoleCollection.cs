using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class RoleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RoleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            RoleElement re = (RoleElement)element;
            return re.Name;
        }
    }
}
