using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Simon.Configuration.Provider
{
	public interface INestedConfigurationElement
	{
		void Init();
		void Deserialize(XmlReader reader);
	}
}
