using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot(Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class CompositeDataField
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public OpenDataType_Type Type { get; set; }

      public CompositeDataField()
      {         
      }

      public CompositeDataField(string name, string description, OpenDataType_Type type)
      {
         Name = name;
         Description = description;
         Type = type;
      }
   }
}