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
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public class PropertyModelInfoType : TypedFeatureInfoType
   {
      public PropertyModelInfoType()
      {
         isis = false;
      }

      [XmlAttribute]
      public string access { get; set; }

      [XmlAttribute("is-is"), DefaultValue(false)]
      public bool isis { get; set; }

      public PropertyModelInfoType(MBeanAttributeInfo attributeInfo)
         : base(attributeInfo)
      {
         access = "";
         if (attributeInfo.Readable)
         {
            access += "r";
         }
         if (attributeInfo.Writable)
         {
            access += "w";
         }
         type = JmxTypeMapping.GetJmxXmlType(attributeInfo.Type);
      }
      public MBeanAttributeInfo Deserialize()
      {
         bool readable = access.IndexOf('r') != -1;
         bool writable = access.IndexOf('w') != -1;
         Descriptor descriptor = GetDescriptorFromFieldValues();
         return new MBeanAttributeInfo(name, Description.Value, JmxTypeMapping.GetCLRTypeName(type), readable, writable,
                                       descriptor);
      }
   }
}