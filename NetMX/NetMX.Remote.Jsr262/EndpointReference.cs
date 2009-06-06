using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   [Serializable()]
   public class EndpointReferenceType : IXmlSerializable, IDeserializable
   {
      private ObjectName _objectName;

      public EndpointReferenceType()
      {         
      }
      
      public EndpointReferenceType(ObjectName objectName)
      {
         _objectName = objectName;
      }

      public ObjectName ObjectName
      {
         get { return _objectName; }
      }

      #region IXmlSerializable Members
      public System.Xml.Schema.XmlSchema GetSchema()
      {
         return null;
      }
      public void ReadXml(XmlReader reader)
      {
         EndpointAddress ea = EndpointAddress.ReadFrom(AddressingVersion.WSAddressing10, reader);
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(ea);         
         _objectName = selectorSet.ExtractObjectName();
      }
      public void WriteXml(XmlWriter writer)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         builder.Uri = OperationContext.Current.Extensions.Find<ServerAddressExtension>().Address;
         builder.Headers.Add(ObjectNameSelector.CreateSelectorSet(ObjectName));         
         EndpointAddress address = builder.ToEndpointAddress();
         address.WriteContentsTo(AddressingVersion.WSAddressing10, XmlDictionaryWriter.CreateDictionaryWriter(writer));
      }
      #endregion

      public object Deserialize()
      {
         return _objectName;
      }
   }
}
