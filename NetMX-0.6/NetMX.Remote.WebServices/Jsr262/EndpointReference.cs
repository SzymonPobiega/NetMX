using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetMX.Remote.WebServices.Jsr262
{
    [Serializable()]
    //[XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
    //[XmlRoot("EndpointReference", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
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
        public void ReadXml(System.Xml.XmlReader reader)
        {
            _address =
                EndpointAddress.ReadFrom(AddressingVersion.WSAddressingAugust2004, XmlDictionaryReader.CreateDictionaryReader(reader));
        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            _address.WriteContentsTo(AddressingVersion.WSAddressingAugust2004, XmlDictionaryWriter.CreateDictionaryWriter(writer));
        }
        #endregion
    }
}
