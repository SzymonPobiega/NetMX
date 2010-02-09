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
   [XmlRoot("ArrayDataType", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class ArrayDataType_Type : OpenDataType_Type, IDeserializable
   {      
      public OpenDataType_Type ElementType { get; set; }

      [XmlAttribute]
      public int dimension { get; set; }

      [XmlIgnore]
      public bool dimensionSpecified { get; set; }

      [XmlAttribute, DefaultValue(false)]
      public bool isPrimitive { get; set; }

      public ArrayDataType_Type()
      {
         isPrimitive = false;
      }

      public ArrayDataType_Type(ArrayType value)
         : base(value)
      {
         dimension = value.Dimension;
         dimensionSpecified = true;
         ElementType = Serialize(value.ElementType);
      }


      public object Deserialize()
      {
         return new ArrayType(dimension, (OpenType) ((IDeserializable) ElementType).Deserialize());
      }
   }
}