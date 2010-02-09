using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using NetMX.OpenMBean;

namespace NetMX.Remote.Jsr262.Structures
{
   [XmlInclude(typeof(TabularDataType_Type))]
   [XmlInclude(typeof(ArrayDataType_Type))]
   [XmlInclude(typeof(CompositeDataType_Type))]
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("SimpleDataType", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class OpenDataType_Type : IDeserializable
   {      
      public string Name { get; set; }
      public XmlQualifiedName Type { get; set; }
      public string Description { get; set; }

      public OpenDataType_Type()
      {         
      }

      public OpenDataType_Type(OpenType value)
      {
         Name = value.TypeName;
         Type = JmxTypeMapping.GetJmxXmlType(value.Representation.AssemblyQualifiedName);
         Description = value.Description;
      }

      public static OpenDataType_Type Serialize(object value)
      {
         ItemChoiceType choiceType;
         return Serialize(value, out choiceType);
      }

      public static OpenDataType_Type Serialize(object value, out ItemChoiceType choiceType)
      {
         Type valueType = value.GetType();
         if (valueType == typeof(TabularType))
         {
            choiceType = ItemChoiceType.TabularDataType;
            return new TabularDataType_Type((TabularType) value);
         }
         if (valueType == typeof(CompositeType))
         {
            choiceType = ItemChoiceType.CompositeDataType;
            return new CompositeDataType_Type((CompositeType)value);
         }
         if (valueType == typeof(ArrayType))
         {
            choiceType = ItemChoiceType.ArrayDataType;
            return new ArrayDataType_Type((ArrayType)value);
         }
         if (valueType == typeof(SimpleType))
         {
            choiceType = ItemChoiceType.SimpleDataType;
            return new OpenDataType_Type((SimpleType) value);
         }
         throw new NotSupportedException("Not supported open type: "+valueType);
      }

      public object Deserialize()
      {
         return SimpleType.CreateSimpleType(System.Type.GetType(JmxTypeMapping.GetCLRTypeName(Type), true));
      }
   }
}