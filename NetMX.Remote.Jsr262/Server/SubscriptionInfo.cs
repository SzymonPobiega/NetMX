using NetMX.Remote.Jsr262.Structures;
using WSMan.NET.Eventing.Server;

namespace NetMX.Remote.Jsr262.Server
{
   public class SubscriptionInfo
   {
      private readonly int _listenerId;
      private readonly IEventSink _eventSink;

      public SubscriptionInfo(IEventSink eventSink, int listenerId)
      {
         _eventSink = eventSink;
         _listenerId = listenerId;
      }

      public IEventSink EventSink
      {
         get { return _eventSink; }
      }

      public int ListenerId
      {
         get { return _listenerId; }
      }

      public void OnNotification(Notification notification, object handback)
      {
         EventSink.Push(new TargetedNotificationType(notification, ListenerId));                                  
      }

      public bool FilterNotification(Notification notification)
      {
         return true; //TODO
      }
   }
}