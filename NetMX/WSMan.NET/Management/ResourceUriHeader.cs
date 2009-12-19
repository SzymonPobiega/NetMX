using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.Management
{
   public class ResourceUriHeader : AddressHeader
   {
      private const string ElementName = "ResourceURI";

      private readonly string _resourceUri;

      public ResourceUriHeader(string resourceUri)
      {
         _resourceUri = resourceUri;
      }

      public static ResourceUriHeader ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, Const.Namespace);
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
         int index = messageHeaders.FindHeader(ElementName, Const.Namespace);
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
         writer.WriteValue(ResourceUri);
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Const.Namespace; }
      }

      public string ResourceUri
      {
         get { return _resourceUri; }
      }
   }
}