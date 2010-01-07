using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.Remote.Jsr262.Client
{
   public struct NotificationSubscriptionKey
   {
      private readonly ObjectName _name;
      private readonly NotificationCallback _callback;
      private readonly NotificationFilterCallback _filterCallback;
      private readonly object _handback;

      public NotificationSubscriptionKey(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         if (name == null)
         {
            throw new ArgumentNullException("name");
         }
         if (callback == null)
         {
            throw new ArgumentNullException("callback");
         }
         _name = name;
         _callback = callback;
         _filterCallback = filterCallback;
         _handback = handback;
      }

      public ObjectName ObjectName
      {
         get { return _name; }
      }

      public NotificationCallback Callback
      {
         get { return _callback; }
      }

      public override bool Equals(object obj)
      {
         if (!(obj is NotificationSubscriptionKey))
         {
            return false;
         }
         NotificationSubscriptionKey other = (NotificationSubscriptionKey) obj;
         return ReferenceEquals(Callback, other.Callback) &&
                ReferenceEquals(_filterCallback, other._filterCallback) &&
                ReferenceEquals(_handback, other._handback) &&
                ObjectName.Equals(other.ObjectName);
      }

      public override int GetHashCode()
      {
         int code = Callback.GetHashCode() ^ ObjectName.GetHashCode();
         if (_filterCallback != null)
         {
            code ^= _filterCallback.GetHashCode();
         }
         if (_handback != null)
         {
            code ^= _handback.GetHashCode();
         }
         return code;
      }
   }
}