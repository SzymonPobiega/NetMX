using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.Transfer
{
   public delegate void HeaderCreatorDelegate(MessageHeaders headers);

   public delegate void AddressHeaderCreatorDelegate(Collection<AddressHeader> addressHeaders);

   public class TransferClient
   {
      private readonly Uri _endpointUri;
      private readonly IChannelFactory<ITransferContract> _proxyFactory;
      private readonly MessageFactory _factory;

      public TransferClient(Uri endpointUri, IChannelFactory<ITransferContract> proxyFactory, MessageVersion version)
      {
         _endpointUri = endpointUri;
         _proxyFactory = proxyFactory;
         _factory = new MessageFactory(version);
      }

      public T Get<T>(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback)
      {
         using (TransferClientContext ctx = new TransferClientContext(_endpointUri, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            return (T)_factory.DeserializeMessageWithPayload(ctx.Channel.Get(_factory.CreateGetRequest()), typeof(T));
         }
      }

      public T Put<T>(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback, object payload)
      {
         using (TransferClientContext ctx = new TransferClientContext(_endpointUri, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);            
            return (T)_factory.DeserializeMessageWithPayload(ctx.Channel.Put(_factory.CreatePutRequest(payload)), typeof(T));
         }
      }

      public EndpointAddress Create(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback, object payload)
      {
         using (TransferClientContext ctx = new TransferClientContext(_endpointUri, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            return _factory.DeserializeCreateResponse(ctx.Channel.Create(_factory.CreateCreateRequest(payload)));
         }
      }

      public void Delete(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback)
      {
         using (TransferClientContext ctx = new TransferClientContext(_endpointUri, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            ctx.Channel.Delete(_factory.CreateDeleteRequest());
         }
      }

      private class TransferClientContext : IDisposable
      {
         private readonly ITransferContract _channel;
         private readonly OperationContextScope _scope;

         public TransferClientContext(Uri endpointUri, IChannelFactory<ITransferContract> proxyFactory, AddressHeaderCreatorDelegate addressHeaderCreatorDelegate)
         {
            EndpointAddressBuilder builder = new EndpointAddressBuilder();
            addressHeaderCreatorDelegate(builder.Headers);
            builder.Uri = endpointUri;

            _channel = proxyFactory.CreateChannel(builder.ToEndpointAddress());
            _scope = new OperationContextScope((IContextChannel)_channel);
         }

         public ITransferContract Channel
         {
            get { return _channel; }
         }

         public void Dispose()
         {
            _scope.Dispose();

            ICommunicationObject comm = (ICommunicationObject)_channel;
            if (comm != null)
            {
               try
               {
                  if (comm.State != CommunicationState.Faulted)
                  {
                     comm.Close();
                  }
                  else
                  {
                     comm.Abort();
                  }
               }
               catch (CommunicationException)
               {
                  comm.Abort();
               }
               catch (TimeoutException)
               {
                  comm.Abort();
               }
               catch (Exception)
               {
                  comm.Abort();
                  throw;
               }
            }
         }
      }
   }
}