using System.ServiceModel.Channels;
using System.Xml;
using NetMX.Remote.Jsr262;

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
         reader.ReadStartElement(ElementName, Schema.EnumerationNamespace);
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
         int index = messageHeaders.FindHeader(ElementName, Schema.EnumerationNamespace);
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
         get { return Schema.EnumerationNamespace; }
      }

      public int Value
      {
         get { return _value; }
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {
         writer.WriteValue(Value);
      }
   }
}