using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET.Eventing;

namespace NetMX.Remote.Jsr262.Client
{
   public class PullSubscriptionListener : IDisposable
   {
      public PullSubscriptionListener(IPullSubscriptionClient<TargetedNotificationType> client, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         _token = new CallbackThreadPoolPullSubscriptionClient<TargetedNotificationType>(
            HandleEvent, client, true);
         
         _filterCallback = filterCallback;
         _handback = handback;
         _callback = callback;         
      }

      private void HandleEvent(TargetedNotificationType result)
      {
         Notification deserializedNotification = Deserialize(result);
         if (_filterCallback == null || _filterCallback(deserializedNotification))
         {
            _callback(deserializedNotification, _handback);
         }
      }

      private static Notification Deserialize(TargetedNotificationType notification)
      {
         return notification.Deserialize();
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