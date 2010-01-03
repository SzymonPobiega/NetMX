using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Eventing;

namespace NetMX.Remote.Jsr262.Server
{
   public class EventingRequestHandler : IEventingRequestHandler<NotificationResult>
   {
      private readonly IMBeanServer _server;

      public EventingRequestHandler(IMBeanServer server)
      {
         _server = server;
      }
      
      public void Bind(IEventingRequestHandlerContext context, EndpointAddressBuilder susbcriptionManagerEndpointAddress)
      {
         susbcriptionManagerEndpointAddress.Headers.Add(new NotificationListenerListHeader("0"));
      }

      public void Unbind(IEventingRequestHandlerContext context)
      {
      }
   }
}