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
   public class FeatureDescriptorTypeField
   {
      public string Name { get; set; }
      public GenericValueType Value { get; set; }

      public FeatureDescriptorTypeField()
      {         
      }

      public FeatureDescriptorTypeField(string name, object value)
      {
         Name = name;
         Value = new GenericValueType(value);
      }
   }
}