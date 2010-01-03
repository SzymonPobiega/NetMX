using System.ServiceModel.Channels;
using System.Xml;

namespace Simon.WsManagement
{
   public class RequestTotalItemsTotalCountEstimate : MessageHeader
   {
      private const string ElementName = "RequestTotalItemsTotalCountEstimate";

      public static bool IsPresent(Message message)
      {
         return IsPresent(message.Headers);
      }

      public static bool IsPresent(MessageHeaders messageHeaders)
      {
         TotalItemsTotalCountEstimate result;
         int index = messageHeaders.FindHeader(ElementName, Schema.Namespace);
         if (index < 0)
         {
            return false;
         }         
         MessageHeaderInfo headerInfo = messageHeaders[index];
         if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
         {
            messageHeaders.UnderstoodHeaders.Add(headerInfo);
         }
         return true;
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Schema.Namespace; }
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {         
      }
   }
}