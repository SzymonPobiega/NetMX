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
   [XmlRoot("TabularDataValue", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class TabularDataValueType : IDeserializable
   {
      public TabularDataValueType()
      {         
      }

      public TabularDataValueType(ITabularData value)
      {
         TabularDataType = new TabularDataType_Type(value.TabularType);
         Values = value.Values.Select(x => new GenericValueType(x)).ToArray();
      }

      public object Deserialize()
      {
         ITabularData result = new TabularDataSupport((TabularType) TabularDataType.Deserialize());
         if (Values != null)
         {
            result.PutAll(Values.Select(x => x.Deserialize()).Cast<ICompositeData>());
         }
         return result;
      }

      [XmlArrayItem("Value", IsNullable = false)]
      public GenericValueType[] Values { get; set; }

      public TabularDataType_Type TabularDataType { get; set; }
   }
}