using System;
using System.Collections.Generic;

namespace NetMX.Timer
{
   /// <summary>
   /// Provides the implementation of the timer MBean. The timer MBean sends out an alarm at a specified time 
   /// that wakes up all the listeners registered to receive timer notifications.
   /// 
   /// This class manages a list of dated timer notifications. A method allows users to add/remove as many 
   /// notifications as required. When a timer notification is emitted by the timer and becomes obsolete, it 
   /// is automatically removed from the list of timer notifications.
   /// 
   /// Additional timer notifications can be added into regularly repeating notifications. 
   /// </summary>
   public class Timer : NotificationEmitterSupport, TimerMBean
   {      
      private int _currentNotificationId;
      private readonly Dictionary<int, TimerNotificationInfo> _notifications = new Dictionary<int, TimerNotificationInfo>();      
      private bool _sendPastNotifications;
      private bool _isActive;      

      public int AddNotification1(string type, string message, object userData, DateTime date)
      {
         return AddNotification4(type, message, userData, date, TimeSpan.FromMilliseconds(-1), 1, false);         
      }

      public int AddNotification2(string type, string message, object userData, DateTime date, TimeSpan period)
      {
         return AddNotification4(type, message, userData, date, period, long.MaxValue, false);
      }

      public int AddNotification3(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences)
      {
         return AddNotification4(type, message, userData, date, period, nbOccurences, false);
      }

      public int AddNotification4(string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences, bool fixedRate)
      {
         int notifId;
         TimerNotificationInfo info;
         lock (_notifications)
         {
            notifId = _currentNotificationId;
            info = new TimerNotificationInfo(notifId, type, message, userData, date, period, nbOccurences,
                                                                   fixedRate, HandleTimerCallback, CheckSendPastNotifications);
            _currentNotificationId++;            
            _notifications[notifId] = info;                        
         }
         if (_isActive)
         {
            info.Start();
         }
         return notifId;
      }

      public IEnumerable<int> GetAllNotificationIDs()
      {
         return _notifications.Keys;
      }

      public IEnumerable<int> GetNotificationIDs(string type)
      {
         lock (_notifications)
         {
            List<int> results = new List<int>();
            foreach (TimerNotificationInfo info in _notifications.Values)
            {
               if (info.Type == type)
               {
                  results.Add(info.NotifificationId);
               }
            }
            return results;
         }
      }

      public DateTime GetDate(int notifiactionId)
      {
         return GetNotifInfo(notifiactionId).Date;
      }

      public bool GetFixedRate(int notificationId)
      {
         return GetNotifInfo(notificationId).IsFixedRate;
      }

      public int NotificationsCount
      {
         get { return _notifications.Count; }
      }

      public long GetNbOccurences(int notificationId)
      {
         return GetNotifInfo(notificationId).NbOccurences;
      }

      public string GetNotificationMessage(int notificationId)
      {
         return GetNotifInfo(notificationId).Message;
      }

      public string GetNotificationType(int notificationId)
      {
         return GetNotifInfo(notificationId).Type;
      }

      public object GetNotificationUserData(int notificationId)
      {
         return GetNotifInfo(notificationId).UserData;
      }

      public TimeSpan GetPeriod(int notificationId)
      {
         return GetNotifInfo(notificationId).Period;
      }

      public bool SendPastNotifications
      {
         get { return _sendPastNotifications; }
         set { _sendPastNotifications = value; }
      }

      public bool IsActive
      {
         get { return _isActive; }
      }

      public bool IsEmpty
      {
         get { return _notifications.Count == 0; }
      }

      public void RemoveAllNotifications()
      {
         lock (_notifications)
         {
            _notifications.Clear();
         }
      }

      public void RemoveNotification(int notificationId)
      {
         lock (_notifications)
         {
            _notifications.Remove(notificationId);
         }
      }
      public void Start()
      {
         if (!_isActive)
         {            
            lock (_notifications)
            {
               foreach (TimerNotificationInfo info in _notifications.Values)
               {
                  info.Start();
               }
            }
            _isActive = true;
         }
      }

      public void Stop()
      {
         if (_isActive)
         {
            _isActive = false;
            lock (_notifications)
            {
               foreach (TimerNotificationInfo info in _notifications.Values)
               {
                  info.Stop();
               }
            }
         }
      }

      #region Utility      
      private void HandleTimerCallback(object state)
      {
         TimerNotificationInfo info = (TimerNotificationInfo) state;
         TimerNotification notif = new TimerNotification(info.Type, this, -1, info.Message, info.UserData, info.NotifificationId);
         SendNotification(notif);
         if (info.NbOccurences == 0)
         {
            _notifications.Remove(info.NotifificationId);
         }
      }
      private bool CheckSendPastNotifications()
      {
         return _sendPastNotifications;
      }
      private TimerNotificationInfo GetNotifInfo(int notificationId)
      {
         lock (_notifications)
         {
            TimerNotificationInfo info;
            if (_notifications.TryGetValue(notificationId, out info))
            {
               return info;
            }
            throw new NotificationNotFoundException(notificationId);
         }
      }      
      #endregion

      private delegate bool CheckSendPastNotificationsDelegate();

      private sealed class TimerNotificationInfo : IDisposable
      {
         private readonly string _type;
         private readonly string _message;
         private readonly object _userData;
         private readonly DateTime _date;
         private readonly TimeSpan _period;
         private readonly int _notificationId;
         private long _nbOccurences;
         private readonly bool _fixedRate;
         private System.Threading.Timer _timer; 
         private readonly System.Threading.TimerCallback _callback;
         private readonly CheckSendPastNotificationsDelegate _sendPastNotifications;

         public TimerNotificationInfo(int notificationId, string type, string message, object userData, DateTime date, TimeSpan period, long nbOccurences, bool fixedRate, System.Threading.TimerCallback callback, CheckSendPastNotificationsDelegate sendPastNotifications)
         {
            _notificationId = notificationId;
            _type = type;
            _message = message;
            _userData = userData;
            _date = date;
            _period = period;
            _nbOccurences = nbOccurences;
            _fixedRate = fixedRate;
            _callback = callback;
            _sendPastNotifications = sendPastNotifications;
         }

         public void Start()
         {                       
            if (_timer == null)
            {               
               DateTime nextNotifDate = _date;
               while (nextNotifDate < DateTime.Now && _nbOccurences > 0)
               {
                  if (_sendPastNotifications())
                  {
                     _callback(this);
                  }
                  _nbOccurences--;
                  nextNotifDate += _period;
               }
               if (_nbOccurences > 0)
               {
                  _timer = new System.Threading.Timer(HandleTimerCallback, this, TimeSpan.Zero, _period);
               }
            }
         }
         public void Stop()
         {
            if (_timer != null)
            {
               _timer.Dispose();
               _timer = null;
            }
         }

         private void HandleTimerCallback(object state)
         {
            _nbOccurences--;
            if (_nbOccurences == 0)
            {
               Stop();
            }
            _callback(this);
         }

         public int NotifificationId
         {
            get { return _notificationId; }
         }

         public string Type
         {
            get { return _type; }
         }

         public string Message
         {
            get { return _message; }
         }

         public object UserData
         {
            get { return _userData; }
         }

         public DateTime Date
         {
            get { return _date; }
         }

         public TimeSpan Period
         {
            get { return _period; }
         }

         public long NbOccurences
         {
            get { return _nbOccurences; }
         }

         public bool IsFixedRate
         {
            get { return _fixedRate; }
         }

         #region IDisposable Members
         public void Dispose()
         {
            Stop();
         }
         #endregion
      }
   }
}
