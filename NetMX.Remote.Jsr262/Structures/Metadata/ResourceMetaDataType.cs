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
   [XmlRoot("DynamicMBeanResourceMetaData", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public partial class ResourceMetaDataType : FeatureDescriptorType
   {
      public Description Description { get; set; }

      [XmlElement("PropertyType")]
      public PropertyModelInfoType[] PropertyType { get; set; }

      [XmlElement("OperationType")]
      public OperationModelInfoType[] OperationType { get; set; }

      [XmlElement("FactoryType")]
      public FactoryModelInfoType[] FactoryType { get; set; }

      [XmlElement("NotificationType")]
      public NotificationModelInfoType[] NotificationType { get; set; }

      public string DynamicMBeanResourceClass { get; set; }

      public ResourceMetaDataType()
      {
      }
      public ResourceMetaDataType(MBeanInfo beanInfo)
         : base(beanInfo.Descriptor)
      {
         Description = new Description { Value = beanInfo.Description };
         DynamicMBeanResourceClass = beanInfo.ClassName;
         FactoryType = beanInfo.Constructors.Select(x => new FactoryModelInfoType(x)).ToArray();
         NotificationType = beanInfo.Notifications.Select(x => new NotificationModelInfoType(x)).ToArray();
         OperationType = beanInfo.Operations.Select(x => new OperationModelInfoType(x)).ToArray();
         PropertyType = beanInfo.Attributes.Select(x => new PropertyModelInfoType(x)).ToArray();
      }
      public MBeanInfo Deserialize()
      {
         return new MBeanInfo(DynamicMBeanResourceClass, Description.Value,
                              PropertyType.EmptyIfNull().Select(x => x.Deserialize()),
                              FactoryType.EmptyIfNull().Select(x => x.Deserialize()),
                              OperationType.EmptyIfNull().Select(x => x.Deserialize()),
                              NotificationType.EmptyIfNull().Select(x => x.Deserialize()), GetDescriptorFromFieldValues());
      } 
   }
}