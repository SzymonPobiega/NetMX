﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262.Client
{
   public interface IDisposableProxy : IJsr262ServiceContract
   {      
   }

   internal sealed class ProxyFactory : IDisposable
   {
      private bool _disposed;
      private readonly ChannelFactory<IJsr262ServiceContract> _channelFactory;
      private readonly Uri _endpointUri;

      public ProxyFactory(ChannelFactory<IJsr262ServiceContract> channelFactory, Uri endpointUri)
      {
         _channelFactory = channelFactory;
         _endpointUri = endpointUri;
      }      

      public Uri EndpointUri
      {
         get { return _endpointUri; }
      }

      public IDisposableProxy Create(string objectName, string resourceUri)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         if (objectName != null)
         {
            builder.Headers.Add(ObjectNameSelector.CreateSelectorSetHeader(objectName));            
         }
         if (resourceUri != null)
         {
            builder.Headers.Add(new ResourceUriHeader(resourceUri));
         }
         builder.Uri = EndpointUri;

         IJsr262ServiceContract proxy = _channelFactory.CreateChannel(builder.ToEndpointAddress());
         OperationContextScope scope = new OperationContextScope((IContextChannel)proxy);
         OperationContext.Current.Extensions.Add(new ServerAddressExtension(EndpointUri));
         return new DisposableProxy(proxy, scope);
      }

      private sealed class DisposableProxy : IDisposableProxy
      {
         private bool _disposed;
         private readonly IJsr262ServiceContract _realProxy;
         private readonly OperationContextScope _scope;

         public DisposableProxy(IJsr262ServiceContract realProxy, OperationContextScope scope)
         {
            _realProxy = realProxy;
            _scope = scope;            
         }
         
         public InvokeResponseMessage Invoke(InvokeMessage requst)
         {
            return _realProxy.Invoke(requst);
         }

         public ResourceMetaDataTypeMessage GetMBeanInfo()
         {
            return _realProxy.GetMBeanInfo();
         }
         
         public IsInstanceOfResponseMessage IsInstanceOf(IsInstanceOfMessage className)
         {
            return _realProxy.IsInstanceOf(className);
         }        
          
         public void Dispose()
         {
            if (!_disposed)
            {
               _disposed = true;
               _scope.Dispose();
               ICommunicationObject co = (ICommunicationObject) _realProxy;
               if (co != null)
               {
                  try
                  {
                     if (co.State != CommunicationState.Faulted)
                     {
                        co.Close();
                     }
                     else
                     {
                        co.Abort();
                     }
                  }
                  catch (CommunicationException)
                  {
                     co.Abort();
                  }
                  catch (TimeoutException)
                  {
                     co.Abort();
                  }
                  catch (Exception)
                  {
                     co.Abort();
                     throw;
                  }
               }
            }
         }
      }

      public void Dispose()
      {
         if (_disposed)
         {
            _channelFactory.Close();
            _disposed = true;
         }
      }
   }
}