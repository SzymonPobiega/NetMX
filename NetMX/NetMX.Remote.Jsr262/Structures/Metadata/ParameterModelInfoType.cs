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
   public class ParameterModelInfoType : TypedFeatureInfoType
   {
      public ParameterModelInfoType()
      {
      }

      public ParameterModelInfoType(string typeName)
      {
         type = JmxTypeMapping.GetJmxXmlType(typeName);
      }

      public ParameterModelInfoType(MBeanParameterInfo parameterInfo)
         : base(parameterInfo)
      {
         type = JmxTypeMapping.GetJmxXmlType(parameterInfo.Type);
      }

      public MBeanParameterInfo Deserialize()
      {
         return new MBeanParameterInfo(name, Description.Value, JmxTypeMapping.GetCLRTypeName(type));
      }
   }
}