using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262
{
    [Serializable()]    
    public class EndpointReferenceType : IXmlSerializable
    {
        private EndpointAddress _address;

        public EndpointAddress a
        {
            get { return _address; }
            set { _address = value; }
        }

        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            _address =
                EndpointAddress.ReadFrom(AddressingVersion.WSAddressing10, XmlDictionaryReader.CreateDictionaryReader(reader));
        }
        public void WriteXml(XmlWriter writer)
        {
            _address.WriteContentsTo(AddressingVersion.WSAddressing10, XmlDictionaryWriter.CreateDictionaryWriter(writer));
        }
        #endregion
    }
}
