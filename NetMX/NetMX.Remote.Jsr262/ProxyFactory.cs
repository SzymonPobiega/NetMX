using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   public interface IDisposableProxy : INetMXWSService, IDisposable
   {      
   }

   internal sealed class ProxyFactory : IDisposable
   {
      private bool _disposed;
      private readonly ChannelFactory<INetMXWSService> _channelFactory;
      private readonly Uri _endpointUri;

      public ProxyFactory(ChannelFactory<INetMXWSService> channelFactory, Uri endpointUri)
      {
         _channelFactory = channelFactory;
         _endpointUri = endpointUri;
      }

      public IDisposableProxy Create(string objectName, string resourceUri)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         if (objectName != null)
         {
            builder.Headers.Add(new SelectorSetHeader(new Selector("ObjectName", objectName)));            
         }
         if (resourceUri != null)
         {
            builder.Headers.Add(new ResourceUriHeader(resourceUri));
         }
         builder.Uri = _endpointUri;

         INetMXWSService proxy = _channelFactory.CreateChannel(builder.ToEndpointAddress());
         OperationContextScope scope = new OperationContextScope((IContextChannel)proxy);
         return new DisposableProxy(proxy, scope);
      }

      private sealed class DisposableProxy : IDisposableProxy
      {
         private bool _disposed;
         private readonly INetMXWSService _realProxy;
         private readonly OperationContextScope _scope;

         public DisposableProxy(INetMXWSService realProxy, OperationContextScope scope)
         {
            _realProxy = realProxy;
            _scope = scope;            
         }

         public DynamicMBeanResource GetAttributes()
         {
            return _realProxy.GetAttributes();
         }

         public DynamicMBeanResource SetAttribute()
         {
            return _realProxy.SetAttribute();
         }

         public void Invoke()
         {
            _realProxy.Invoke();
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
