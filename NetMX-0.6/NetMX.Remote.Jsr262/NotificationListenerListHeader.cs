using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace NetMX.Remote.Jsr262
{
   public class NotificationListenerListHeader : AddressHeader
   {
      private const string ElementName = "NotificationListenerList";

      private readonly string _value;

      public NotificationListenerListHeader(string value)
      {
         _value = value;
      }

      public static NotificationListenerListHeader GetFrom(AddressHeaderCollection headerCollection)
      {
         return (NotificationListenerListHeader)headerCollection.FindHeader(ElementName, Schema.ConnectorNamespace);
      }

      public static NotificationListenerListHeader ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, Schema.ConnectorNamespace);
         string result = reader.Value;
         reader.Read();
         reader.ReadEndElement();
         return new NotificationListenerListHeader(result);
      }

      public static NotificationListenerListHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static NotificationListenerListHeader ReadFrom(MessageHeaders messageHeaders)
      {
         NotificationListenerListHeader result;
         int index = messageHeaders.FindHeader(ElementName, Schema.ConnectorNamespace);
         if (index < 0)
         {
            return null;
         }
         using (XmlDictionaryReader readerAtHeader = messageHeaders.GetReaderAtHeader(index))
         {
            result = ReadFrom(readerAtHeader);
         }
         MessageHeaderInfo headerInfo = messageHeaders[index];
         if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
         {
            messageHeaders.UnderstoodHeaders.Add(headerInfo);
         }
         return result;
      }

      protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
      {
         writer.WriteValue(_value);
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Schema.ConnectorNamespace; }
      }

      public string Value
      {
         get { return _value; }
      }
   }
}
