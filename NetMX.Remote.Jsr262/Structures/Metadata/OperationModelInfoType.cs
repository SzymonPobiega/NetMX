using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public class OperationModelInfoType : FeatureInfoType
   {
      [XmlElement("Input")]
      public ParameterModelInfoType[] Input { get; set; }

      public ParameterModelInfoType Output { get; set; }

      [XmlAttribute]
      public string impact { get; set; }

      public OperationModelInfoType()
      {         
      }

      public OperationModelInfoType(MBeanOperationInfo operationInfo) : base(operationInfo)
      {
         Input = operationInfo.Signature.Select(x => new ParameterModelInfoType(x)).ToArray();
         impact = "";
         if ((operationInfo.Impact & OperationImpact.Info) == OperationImpact.Info)
         {
            impact += "r";
         }
         if ((operationInfo.Impact & OperationImpact.Action) == OperationImpact.Action)
         {
            impact += "w";
         }
         if (impact.Length == 0)
         {
            impact = "unknown";
         }
         if (operationInfo.ReturnType != typeof(void).AssemblyQualifiedName)
         {
            Output = new ParameterModelInfoType(operationInfo.ReturnType);  
         }         
      }
      public MBeanOperationInfo Deserialize()
      {
         OperationImpact impactEnum = OperationImpact.Unknown;
         if (impact != null && impact.IndexOf('r') != -1)
         {
            impactEnum |= OperationImpact.Info;
         }
         if (impact != null && impact.IndexOf('w') != -1)
         {
            impactEnum |= OperationImpact.Action;
         }
         XmlQualifiedName typeQualifiedName = null;
         if (Output != null)
         {
            typeQualifiedName = Output.type;
         }
         return new MBeanOperationInfo(name, Description.Value, JmxTypeMapping.GetCLRTypeName(typeQualifiedName),
                                       Input.EmptyIfNull().Select(x => x.Deserialize()).ToArray(),
                                       impactEnum, GetDescriptorFromFieldValues());
      }      
   }
}