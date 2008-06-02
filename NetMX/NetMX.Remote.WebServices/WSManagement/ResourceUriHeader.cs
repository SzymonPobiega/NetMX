using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.WebServices.WSManagement
{
    public static class ResourceUriHeader
    {
        private const string ElementName = "ResourceURI";

        public static AddressHeader Create(string resourceUri)
        {
            return AddressHeader.CreateAddressHeader(ElementName, WSMan.WSManagementNamespace, resourceUri);
        }

        public static string ReadFrom(XmlDictionaryReader reader)
        {
            reader.ReadStartElement(ElementName, WSMan.WSManagementNamespace);
            string result = reader.Value;
            reader.Read();
            reader.ReadEndElement();
            return result;
        }

        public static string ReadFrom(Message message)
        {
            string result;
            int index = message.Headers.FindHeader(ElementName, WSMan.WSManagementNamespace);
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
