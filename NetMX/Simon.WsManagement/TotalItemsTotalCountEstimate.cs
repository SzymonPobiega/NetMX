using System.ServiceModel.Channels;
using System.Xml;

namespace Simon.WsManagement
{   
   public sealed class TotalItemsTotalCountEstimate : MessageHeader
   {
      private const string ElementName = "TotalItemsTotalCountEstimate";

      private readonly int _value;

      public TotalItemsTotalCountEstimate(int value)
      {
         _value = value;
      }

      public static TotalItemsTotalCountEstimate ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, Schema.Namespace);
         int value = XmlConvert.ToInt32(reader.ReadString());
         TotalItemsTotalCountEstimate result = new TotalItemsTotalCountEstimate(value);
         reader.ReadEndElement();
         return result;
      }

      public static TotalItemsTotalCountEstimate ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static TotalItemsTotalCountEstimate ReadFrom(MessageHeaders messageHeaders)
      {
         TotalItemsTotalCountEstimate result;
         int index = messageHeaders.FindHeader(ElementName, Schema.Namespace);
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

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return WsTransfer.Namespace; }
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {
         writer.WriteValue(_value);
      }
   }
}