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
   public class NotificationModelInfoType : TypedFeatureInfoType
   {
      [XmlElement("NotificationType")]
      public string[] NotificationType { get; set; }

      public NotificationModelInfoType()
      {
      }
      public NotificationModelInfoType(MBeanNotificationInfo notificationInfo)
         : base(notificationInfo)
      {         
         NotificationType = notificationInfo.NotifTypes.ToArray();
      }
      public MBeanNotificationInfo Deserialize()
      {
         return new MBeanNotificationInfo(NotificationType, name, Description.Value);
      }
   }
}