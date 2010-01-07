using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Eventing;

namespace NetMX.Remote.Jsr262.Client
{
   public class PullSubscriptionListener : IDisposable
   {      
      public PullSubscriptionListener(IPullSubscriptionClient<NotificationResult> client, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         _token = new CallbackThreadPoolPullSubscriptionClient<NotificationResult>(
            HandleEvent, client, true);
         
         _filterCallback = filterCallback;
         _handback = handback;
         _callback = callback;         
      }      

      private void HandleEvent(NotificationResult result)
      {
         foreach (TargetedNotificationType targetedNotification in result.TargetedNotification)
         {
            //targetedNotification.
         }
      }

      public void Dispose()
      {
         if (_diposed)
         {
            return;            
         }
         _token.Dispose();
         _diposed = true;
      }

      private readonly IDisposable _token;
      private readonly NotificationCallback _callback;
      private readonly NotificationFilterCallback _filterCallback;
      private readonly object _handback;
      private bool _diposed;
   }
}