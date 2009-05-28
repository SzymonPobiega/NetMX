using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262
{
   [Serializable]
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot(Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class GetDefaultDomainResponse
   {      
      [XmlText]
      public string DomainName
      {
         get; set;
      }
   }

   [Serializable]
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot(Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class GetDomainsResponse
   {
      [XmlElement("Domain")]
      public string[] DomainNames
      {
         get;
         set;
      }
   }
}
