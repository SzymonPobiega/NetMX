using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.Transfer
{
   public class CreateRsponseBodyWriter : BodyWriter
   {
      private readonly EndpointAddress _body;
      private readonly AddressingVersion _version;

      public CreateRsponseBodyWriter(EndpointAddress body, MessageVersion version)
         : base(false)
      {
         _body = body;
         _version = version.Addressing;
      }

      protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
      {
         writer.WriteStartElement(Const.CreateResponse_ResourceCreatedElement);         
         _body.WriteContentsTo(_version, writer);
         writer.WriteEndElement();
      }
   }
}