using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures
{
   [XmlInclude(typeof(NotificationModelInfoType))]
   [XmlInclude(typeof(ParameterModelInfoType))]
   [XmlInclude(typeof(PropertyModelInfoType))]
   [XmlInclude(typeof(PropertyType))]
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]

   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public abstract class TypedFeatureInfoType : FeatureInfoType
   {
      protected TypedFeatureInfoType()
      {
         table = 0;
      }

      protected TypedFeatureInfoType(MBeanFeatureInfo featureInfo)
         : base(featureInfo)
      {
      }

      [XmlAttribute]
      public XmlQualifiedName type { get; set; }

      [XmlAttribute, DefaultValue(0)]
      public int table { get; set; }

      [XmlAttribute]
      public bool primitive { get; set; }

      [XmlIgnore]
      public bool primitiveSpecified { get; set; }
   }
}