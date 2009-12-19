using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Transfer
{
   public class MessageFactory
   {      
      private readonly MessageVersion _version;

      public MessageFactory(MessageVersion version)
      {
         _version = version;
      }

      public Message CreateGetRequest()
      {
         return CreateMessageWithPayload(null, Const.GetAction);
      }

      public Message CreateGetResponse(object body)
      {
         return CreateMessageWithPayload(body, Const.GetResponseAction);
      }

      public Message CreatePutRequest(object payload)
      {
         return CreateMessageWithPayload(payload, Const.PutAction);
      }                   

      public Message CreatePutResponse(object payload)
      {
         return CreateMessageWithPayload(payload, Const.PutResponseAction);
      }

      public Message CreateCreateRequest(object payload)
      {         
         return CreateMessageWithPayload(payload, Const.CreateAction);
      }

      public Message CreateCreateResponse(EndpointAddress result)
      {
         return Message.CreateMessage(_version, Const.CreateResponseAction,
                                      new CreateRsponseBodyWriter(result, _version));
      }

      public Message CreateDeleteRequest()
      {
         return CreateMessageWithPayload(null, Const.DeleteAction);
      }

      public Message CreateDeleteResponse()
      {
         return CreateMessageWithPayload(null, Const.DeleteResponseAction);
      }

      public EndpointAddress DeserializeCreateResponse(Message createResponseMessage)
      {         
         XmlDictionaryReader reader = createResponseMessage.GetReaderAtBodyContents();
         reader.ReadStartElement(Const.CreateResponse_ResourceCreatedElement, Const.Namespace);

         EndpointAddress result = EndpointAddress.ReadFrom(reader);

         if (reader.NodeType == XmlNodeType.EndElement)
         {
            reader.ReadEndElement();
         }

         return result;
      }

      public object DeserializeMessageWithPayload(Message messageWithPayload, Type expectedType)
      {
         if (messageWithPayload.IsEmpty)
         {
            return null;
         }
         if (typeof(IXmlSerializable).IsAssignableFrom(expectedType))
         {
            IXmlSerializable serializable = (IXmlSerializable)Activator.CreateInstance(expectedType);
            serializable.ReadXml(messageWithPayload.GetReaderAtBodyContents());
            return serializable;
         }
         XmlSerializer xs = new XmlSerializer(expectedType);         
         return xs.Deserialize(messageWithPayload.GetReaderAtBodyContents());
      }

      public Message CreateMessageWithPayload(object payload, string action)
      {
         Message respose = Message.CreateMessage(_version, action, new SerializerBodyWriter(payload));         
         return respose;
      }      
   }
}