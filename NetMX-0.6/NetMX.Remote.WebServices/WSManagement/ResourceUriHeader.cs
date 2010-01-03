using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.WebServices.WSManagement
{
   public class ResourceUriHeader : MessageHeader
   {
      private const string ElementName = "ResourceURI";

      private readonly string _resourceUri;

      public ResourceUriHeader(string resourceUri)
      {
         _resourceUri = resourceUri;
      }

      public static ResourceUriHeader ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, WSMan.WSManagementNamespace);
         string result = reader.Value;
         reader.Read();
         reader.ReadEndElement();
         return new ResourceUriHeader(result);
      }

      public static ResourceUriHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static ResourceUriHeader ReadFrom(MessageHeaders messageHeaders)
      {
         ResourceUriHeader result;
         int index = messageHeaders.FindHeader(ElementName, WSMan.WSManagementNamespace);
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
         get { return WSMan.WSManagementNamespace; }
      }

      public string ResourceUri
      {
         get { return _resourceUri; }
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {
         writer.WriteValue(ResourceUri);
      }
   }
}
