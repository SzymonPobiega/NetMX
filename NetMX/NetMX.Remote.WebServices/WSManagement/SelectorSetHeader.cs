using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.WebServices.WSManagement
{
   public sealed class SelectorSetHeader : AddressHeader
   {
      internal const string ElementName = "SelectorSet";
      private readonly List<Selector> _selectors;

      public override string Name
      {
         get { return ElementName; }
      }
      public override string Namespace
      {
         get { return WSMan.WSManagementNamespace; }
      }

      public List<Selector> Selectors
      {
         get { return _selectors; }
      }

      public SelectorSetHeader(params Selector[] selectors)
      {
         _selectors = new List<Selector>(selectors);
      }

      public SelectorSetHeader(IEnumerable<Selector> selectors)
      {
         _selectors = new List<Selector>(selectors);
      }

      private SelectorSetHeader()
      {
         _selectors = new List<Selector>();
      }
      
      public static SelectorSetHeader ReadFrom(XmlDictionaryReader reader)
      {
         SelectorSetHeader result = new SelectorSetHeader();
         reader.ReadStartElement(ElementName, WSMan.WSManagementNamespace);
         while (reader.Name == Selector.ElementName)
         {
            Selector newSelector = Selector.ReadFrom(reader);
            result.Selectors.Add(newSelector);
         }
         reader.ReadEndElement();
         return result;
      }
      public static SelectorSetHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static SelectorSetHeader ReadFrom(MessageHeaders messageHeaders)
      {
         SelectorSetHeader result;
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
      
      protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
      {
         foreach (Selector selector in Selectors)
         {
            selector.WriteTo(writer);
         }
      }
   }
}
