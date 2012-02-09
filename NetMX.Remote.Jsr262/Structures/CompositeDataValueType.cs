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
   [XmlRoot("CompositeDataValue", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class CompositeDataValueType : IDeserializable
   {
      public CompositeDataValueType()
      {         
      }

      public CompositeDataValueType(ICompositeData value)
      {
         CompositeDataType = new CompositeDataType_Type(value.CompositeType);
         Values = value.Values.Select(x => new GenericValueType(x)).ToArray();
      }

      public object Deserialize()
      {
         CompositeType type = (CompositeType) CompositeDataType.Deserialize();
         return new CompositeDataSupport(type, type.KeySet.OrderBy(x => x), Values.Select(x => x.Deserialize()));
      }

      [XmlArrayItem("Value", IsNullable = false)]
      public GenericValueType[] Values { get; set; }

      public CompositeDataType_Type CompositeDataType { get; set; }      
   }
}