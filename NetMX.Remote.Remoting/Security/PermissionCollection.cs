using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NetMX.Remote.Remoting.Security
{
    public class PermissionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            PermissionElement pe = (PermissionElement)element;
            return pe.Pattern;
        }
    }
}
