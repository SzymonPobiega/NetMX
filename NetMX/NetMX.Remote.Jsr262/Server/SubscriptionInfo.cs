using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET.Eventing;

namespace NetMX.Remote.Jsr262.Server
{
   public class SubscriptionInfo
   {
      private readonly int _listenerId;
      private readonly IEventingRequestHandlerContext _eventingContext;

      public SubscriptionInfo(IEventingRequestHandlerContext eventingContext, int listenerId)
      {
         _eventingContext = eventingContext;
         _listenerId = listenerId;
      }

      public IEventingRequestHandlerContext EventingContext
      {
         get { return _eventingContext; }
      }

      public int ListenerId
      {
         get { return _listenerId; }
      }

      public void OnNotification(Notification notification, object handback)
      {
         EventingContext.Push(new TargetedNotificationType(notification, ListenerId));                                  
      }

      public bool FilterNotification(Notification notification)
      {
         return true; //TODO
      }
   }
}