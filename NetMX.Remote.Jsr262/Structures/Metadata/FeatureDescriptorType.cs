using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures
{
   [XmlInclude(typeof(ResourceMetaDataType))]
   [XmlInclude(typeof(FeatureInfoType))]
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
   public abstract class FeatureDescriptorType
   {
      [XmlElement("Field")]
      public List<FeatureDescriptorTypeField> Field { get; set; }

      protected FeatureDescriptorType()
      {         
      }

      protected FeatureDescriptorType(Descriptor descriptor)
      {
         SetFieldValuesFromDescriptor(descriptor);
      }

      private void SetFieldValuesFromDescriptor(Descriptor descriptor)
      {
         Field = new List<FeatureDescriptorTypeField>();
         foreach (string fieldName in descriptor.GetFieldNames())
         {
            Field.Add(new FeatureDescriptorTypeField(fieldName, descriptor.GetFieldValue(fieldName)));
         }
      }

      protected Descriptor GetDescriptorFromFieldValues()
      {
         Descriptor descriptor = new Descriptor();
         if (Field != null)
         {
            foreach (FeatureDescriptorTypeField field in Field)
            {
               descriptor.SetField(field.Name, field.Value.Deserialize());
            }
         }
         return descriptor;
      }
   }
}