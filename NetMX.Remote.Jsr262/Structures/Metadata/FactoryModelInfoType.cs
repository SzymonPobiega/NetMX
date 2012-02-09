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
   public class FactoryModelInfoType : FeatureInfoType
   {
      [XmlElement("Parameter")]
      public ParameterModelInfoType[] Parameter { get; set; }

      public FactoryModelInfoType()
      {
      }

      public FactoryModelInfoType(MBeanConstructorInfo constructorInfo)
         : base(constructorInfo)
      {
         Parameter = constructorInfo.Signature.Select(x => new ParameterModelInfoType(x)).ToArray();
      }
      public MBeanConstructorInfo Deserialize()
      {
         return new MBeanConstructorInfo(name, Description.Value,
                                         Parameter.EmptyIfNull().Select(x => x.Deserialize()).ToArray());
      }
   }
}