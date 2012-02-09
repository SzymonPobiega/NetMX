using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NetMX.Configuration.Provider;

namespace NetMX.Remote.Remoting.Security
{
    public class RoleCollection : ConfigurationElementCollection, INestedConfigurationElement
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

		  #region INestedConfigurationElement Members
		  void INestedConfigurationElement.Init()
		  {
			  this.Init();
		  }

		  void INestedConfigurationElement.Deserialize(System.Xml.XmlReader reader)
		  {
			  this.DeserializeElement(reader, true);
		  }
		  #endregion
	  }
}
