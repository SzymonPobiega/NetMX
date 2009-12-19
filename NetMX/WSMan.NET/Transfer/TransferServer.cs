using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.Transfer
{   
   public class TransferServer : ITransferContract
   {
      private readonly ITransferRequestHandler _handler;
      private readonly MessageFactory _factory;

      public TransferServer(ITransferRequestHandler handler, MessageVersion version)
      {
         _handler = handler;
         _factory = new MessageFactory(version);
      }

      public Message Get(Message getRequest)
      {
         object payload = _handler.HandleGet();
         return _factory.CreateGetResponse(payload);
      }

      public Message Put(Message putRequest)
      {
         object payload = _handler.HandlePut(x => _factory.DeserializeMessageWithPayload(putRequest, x));
         return _factory.CreatePutResponse(payload);
      }

      public Message Create(Message createRequest)
      {
         EndpointAddress address = _handler.HandleCreate(x => _factory.DeserializeMessageWithPayload(createRequest, x));
         return _factory.CreateCreateResponse(address);
      }

      public Message Delete(Message deleteRequest)
      {
         _handler.HandlerDelete();
         return _factory.CreateDeleteResponse();
      }
   }
}