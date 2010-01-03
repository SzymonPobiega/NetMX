using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace NetMX.Remote.WebServices.WSManagement
{
   [DataContract(Namespace = WSMan.WSManagementNamespace)]
   public sealed class XmlFragment : IXmlSerializable
   {
      #region IXmlSerializable Members
      public System.Xml.Schema.XmlSchema GetSchema()
      {
         return null;
      }
      public void ReadXml(System.Xml.XmlReader reader)
      {
      }
      public void WriteXml(System.Xml.XmlWriter writer)
      {
      }
      #endregion
   }
}
