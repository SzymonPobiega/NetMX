using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262
{
   [Serializable]
   [XmlRoot("XmlFragment", Namespace = Simon.WsManagement.Schema.Namespace)]
   public class DynamicMBeanResourceFragment
   {
      private NamedGenericValueType[] propertyField;

      /// <remarks/>
      [XmlElement("Property")]
      public NamedGenericValueType[] Property
      {
         get { return propertyField; }
         set { propertyField = value; }
      }
   }

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
