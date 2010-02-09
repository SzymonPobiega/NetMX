using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using NetMX.OpenMBean;

namespace NetMX.Remote.Jsr262.Structures
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("TabularDataType", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class TabularDataType_Type : OpenDataType_Type, IDeserializable
   {
      public CompositeDataType_Type CompositeType { get; set; }

      [XmlElement("index")]
      public string[] index { get; set; }

      public TabularDataType_Type()
      {
         
      }
      public TabularDataType_Type(TabularType value)
         : base(value)
      {
         index = value.IndexNames.ToArray();
         CompositeType = new CompositeDataType_Type(value.RowType);
      }

      public object Deserialize()
      {
         return new TabularType(Name, Description, (CompositeType)CompositeType.Deserialize(), index);
      }
   }
}