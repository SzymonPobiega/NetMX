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
   [XmlRoot("CompositeDataType", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class CompositeDataType_Type : OpenDataType_Type, IDeserializable
   {
      [XmlElement("CompositeDataField")]
      public CompositeDataField[] CompositeDataField { get; set; }

      public CompositeDataType_Type()
      {
      }

      public CompositeDataType_Type(CompositeType value)
         : base(value)
      {
         List<CompositeDataField> fields = new List<CompositeDataField>();
         foreach (string fieldName in value.KeySet)
         {            
            fields.Add(new CompositeDataField(fieldName, value.GetDescription(fieldName), Serialize(value.GetOpenType(fieldName))));
         }
         CompositeDataField = fields.ToArray();
      }      

      public object Deserialize()
      {
         return new CompositeType(Name, Description, 
            CompositeDataField.EmptyIfNull().Select(x => x.Name),
            CompositeDataField.EmptyIfNull().Select(x => x.Description),
            CompositeDataField.EmptyIfNull().Select(x => (OpenType)((IDeserializable)x.Type).Deserialize()));
      }
   }
}