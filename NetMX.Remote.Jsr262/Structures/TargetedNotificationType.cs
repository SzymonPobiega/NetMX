using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using WSMan.NET;

namespace NetMX.Remote.Jsr262.Structures
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("TargetedNotification", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class TargetedNotificationType
   {
      public EndpointReference Emitter { get; set; }

      public GenericValueType UserData { get; set; }

      public string Message { get; set; }

      [XmlAttribute]
      public int listenerId { get; set; }

      [XmlAttribute]
      public string notificationClass { get; set; }

      [XmlAttribute]
      public string eventType { get; set; }

      [XmlAttribute]
      public long sequenceNumber { get; set; }      

      [XmlAttribute]
      public long timeStamp { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }

      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      public TargetedNotificationType()
      {         
      }

      public TargetedNotificationType(Notification notification, int listenerId)
      {
         this.listenerId = listenerId;
         eventType = notification.Type;
         timeStamp = notification.Timestamp.Ticks;
         Message = notification.Message;
         sequenceNumber = notification.SequenceNumber;
         notificationClass = notification.GetType().AssemblyQualifiedName;
         UserData = new GenericValueType(notification.UserData);
      }

      public Notification Deserialize()
      {
         //TODO: subclasses.
         return new Notification(eventType, null, sequenceNumber, Message, UserData.Deserialize());
      }
   }
}