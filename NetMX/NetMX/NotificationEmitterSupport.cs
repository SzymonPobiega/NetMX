using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace NetMX
{
    public class NotificationEmitterSupport : INotficationEmitter
    {
        #region Members
        private string _objectName;
        private IList<MBeanNotificationInfo> _notificationInfo;
        private Dictionary<NotificationSubscription, List<NotificationSubscription>> _subscriptions = new Dictionary<NotificationSubscription, List<NotificationSubscription>>();
        #endregion   
     
        #region Interface
        /// <summary>
        /// Initializes this emitter. Should be called in PreRegister phase of inheriting or owning MBean.
        /// </summary>
        /// <param name="objectName">ObjectName of inheriting or owning MBean</param>
        /// <param name="notificationInfo">NotificationInfo list of inheriting or owning MBean</param>
        public void Initialize(string objectName, IList<MBeanNotificationInfo> notificationInfo)
        {
            _notificationInfo = notificationInfo;
            _objectName = objectName;
        }
        /// <summary>
        /// Sends a notification. 
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public void SendNotification(Notification notification)
        {
            foreach (List<NotificationSubscription> subscrList in _subscriptions.Values)
            {
                foreach (NotificationSubscription subscr in subscrList)
                {
                    if (subscr.FilterCallback == null || subscr.FilterCallback(notification))
                    {
                        HandleNotification(subscr.Callback, notification, subscr.Handback);
                    }
                }
            }
        }
        /// <summary>
        /// This method is called by <see cref="SendNotification(Notification)"/> for each listener in order to send the notification to that 
        /// listener. It can be overridden in subclasses to change the behavior of notification delivery, for 
        /// instance to deliver the notification in a separate thread.
        /// </summary>
        /// <remarks>
        /// It is not guaranteed that this method is called by the same thread as the one that called 
        /// <see cref="SendNotification(Notification)"/>.
        /// The default implementation of this method is equivalent to <code>callback(notification, handback);</code>        
        /// </remarks>
        /// <param name="callback"></param>
        /// <param name="notification"></param>
        /// <param name="handback"></param>
        protected virtual void HandleNotification(NotificationCallback callback, Notification notification, object handback)
        {
            callback(notification, handback);
        }
        #endregion

        #region INotficationEmitter Members
        public void AddNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            NotificationSubscription subscr = new NotificationSubscription(callback, filterCallback, handback);
            List<NotificationSubscription> subscrList;
            if (_subscriptions.TryGetValue(subscr, out subscrList))
            {
                subscrList.Add(subscr);
            }
            else
            {
                subscrList = new List<NotificationSubscription>();
                subscrList.Add(subscr);
                _subscriptions.Add(subscr, subscrList);
            }
        }
        public void RemoveNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            NotificationSubscription subscr = new NotificationSubscription(callback, filterCallback, handback);
            if (!_subscriptions.Remove(subscr))
            {
                throw new ListenerNotFoundException(_objectName);
            }
        }
        public void RemoveNotificationListener(NotificationCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            List<NotificationSubscription> toRemove = new List<NotificationSubscription>();
            foreach (NotificationSubscription subscr in _subscriptions.Keys)
            {
                if (subscr.Callback.Equals(callback))
                {
                    toRemove.Add(subscr);
                }
            }
            if (toRemove.Count == 0)
            {
                throw new ListenerNotFoundException(_objectName);
            }
            foreach (NotificationSubscription subscr in toRemove)
            {
                _subscriptions.Remove(subscr);
            }
        }
        public IList<MBeanNotificationInfo> NotificationInfo
        {
            get { return _notificationInfo; }
        }
        #endregion

        #region Helper class
        private class NotificationSubscription
        {
            private NotificationCallback _callback;
            public NotificationCallback Callback
            {
                get { return _callback; }
            }
            private NotificationFilterCallback _filterCallback;
            public NotificationFilterCallback FilterCallback
            {
                get { return _filterCallback; }
            }
            private object _handback;
            public object Handback
            {
                get { return _handback; }
            }

            public NotificationSubscription(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
            {
                _callback = callback;
                _filterCallback = filterCallback;
                _handback = handback;
            }

            public override bool Equals(object obj)
            {
                NotificationSubscription other = obj as NotificationSubscription;
                if (other != null)
                {
                    return this._callback.Equals(other._callback) &&
                        ( (this._filterCallback == null && other._filterCallback == null) ||
                        (this._filterCallback != null && other._filterCallback != null && this._filterCallback.Equals(other._filterCallback))) &&
                        ((this._handback == null && other._handback == null) ||
                        (this._handback != null && other._handback != null && this._handback.Equals(other._handback)));
                }
                return false;
            }

            public override int GetHashCode()
            {
                int code = _callback.GetHashCode();
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
        #endregion
    }
}
