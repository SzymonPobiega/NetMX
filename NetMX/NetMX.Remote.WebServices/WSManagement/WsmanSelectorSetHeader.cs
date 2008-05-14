using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.WebServices.WSManagement
{
    internal class WsmanSelectorSetHeader : AddressHeader
    {
        private readonly List<Selector> _selectors;

        public override string Name
        {
            get { return "SelectorSet"; }
        }
        public override string Namespace
        {
            get { return WSMan.WSManagementNamespace; }
        }

        public WsmanSelectorSetHeader(params Selector[] selectors)
        {
            _selectors = new List<Selector>(selectors);
        }

        public WsmanSelectorSetHeader(IEnumerable<Selector> selectors)
        {
            _selectors = new List<Selector>(selectors);    
        }

        private WsmanSelectorSetHeader()
        {
            _selectors = new List<Selector>();
        }
        
        protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
        {
            //writer.WriteStartElement(Name, Namespace);
            foreach (Selector selector in _selectors)
            {
                selector.WriteTo(writer);
            }
            //writer.WriteEndElement();
        }
        public static WsmanSelectorSetHeader ReadFrom(XmlDictionaryReader reader)
        {
            WsmanSelectorSetHeader result = new WsmanSelectorSetHeader();
            reader.ReadStartElement("SelectorSet", WSMan.WSManagementNamespace);
            while (reader.Name == "Selector")
            {
                Selector newSelector = Selector.ReadFrom(reader);
                result._selectors.Add(newSelector);
            }
            reader.ReadEndElement();
            return result;
        }
        public static WsmanSelectorSetHeader ReadFrom(Message message)
        {            
            WsmanSelectorSetHeader result;
            int index = message.Headers.FindHeader("SelectorSet", WSMan.WSManagementNamespace);
            if (index < 0)
            {
                return null;
            }
            using (XmlDictionaryReader readerAtHeader = message.Headers.GetReaderAtHeader(index))
            {
                result = ReadFrom(readerAtHeader);
            }
            MessageHeaderInfo headerInfo = message.Headers[index];
            if (!message.Headers.UnderstoodHeaders.Contains(headerInfo))
            {
                message.Headers.UnderstoodHeaders.Add(headerInfo);
            }
            return result;
        }
    }
}
