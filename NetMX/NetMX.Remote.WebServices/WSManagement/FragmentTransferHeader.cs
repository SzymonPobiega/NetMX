using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.WebServices.WSManagement
{
    public sealed class FragmentTransferHeader : MessageHeader
    {
        private readonly string _expression;

        public FragmentTransferHeader(string expression)
        {
            _expression = expression;
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteValue(_expression);
        }

        public static FragmentTransferHeader ReadFrom(XmlDictionaryReader reader)
        {            
            reader.ReadStartElement("XmlFragment", WSMan.WSManagementNamespace);
            FragmentTransferHeader result = new FragmentTransferHeader(reader.Value);
            reader.Read();
            reader.ReadEndElement();
            return result;
        }

        public static FragmentTransferHeader ReadFrom(Message message)
        {
            FragmentTransferHeader result;
            int index = message.Headers.FindHeader("XmlFragment", WSMan.WSManagementNamespace);
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

        public override string Name
        {
            get { return "XmlFragment"; }
        }

        public override string Namespace
        {
            get { return WSMan.WSManagementNamespace; }
        }

        public string Expression
        {
            get { return _expression; }
        }
    }
}
