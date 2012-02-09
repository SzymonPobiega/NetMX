using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures
{
   [XmlInclude(typeof(FactoryModelInfoType))]
   [XmlInclude(typeof(OperationModelInfoType))]
   [XmlInclude(typeof(TypedFeatureInfoType))]
   [XmlInclude(typeof(NotificationModelInfoType))]
   [XmlInclude(typeof(ParameterModelInfoType))]
   [XmlInclude(typeof(PropertyModelInfoType))]
   [XmlInclude(typeof(PropertyType))]
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]

   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public abstract class FeatureInfoType : FeatureDescriptorType
   {
      public Description Description { get; set; }

      [XmlAttribute]
      public string name { get; set; }

      protected FeatureInfoType()
      {
      }

      protected FeatureInfoType(MBeanFeatureInfo featureInfo) 
         : base(featureInfo.Descriptor)
      {
         name = featureInfo.Name;
         Description = new Description { Value = featureInfo.Description };
      } 
   }
}