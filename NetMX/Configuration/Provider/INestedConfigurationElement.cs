using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NetMX.Configuration.Provider
{
   public interface INestedConfigurationElement
   {
      void Init();
      void Deserialize(XmlReader reader);
   }
}