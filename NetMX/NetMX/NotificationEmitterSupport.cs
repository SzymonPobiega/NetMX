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
		private ReadOnlyCollection<MBeanNotificationInfo> _notificationInfo;
		private Dictionary<NotificationSubscription, List<NotificationSubscription>> _subscriptions = new Dictionary<NotificationSubscription, List<NotificationSubscription>>();
		private int _sequenceNumber;
		private readonly object _sequenceNumberSynch = new object();
		#endregion

		#region Interface
		/// <summary>
		/// Initializes this emitter. Should be called in PreRegister phase of inheriting or owning MBean.
		/// </summary>
		/// <param name="objectName">ObjectName of inheriting or owning MBean</param>
		/// <param name="notificationInfo">NotificationInfo list of inheriting or owning MBean</param>
		public void Initialize(string objectName, IEnumerable<MBeanNotificationInfo> notificationInfo)
		{
			_notificationInfo = new List<MBeanNotificationInfo>(notificationInfo).AsReadOnly();
			_objectName = objectName;
		}
		/// <summary>
		/// Sends a notification. 
		/// </summary>
		/// <param name="notification">The notification to send.</param>
		public void SendNotification(Notification notification)
		{
			if (notification.SequenceNumber == -1)
			{
				notification.SequenceNumber = GetNextSequenceNumber();
			}
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
			callback((Notification) notification.Clone(), handback);
		}
		#endregion

		#region Utility
		private int GetNextSequenceNumber()
		{
			lock (_sequenceNumberSynch)
			{
				_sequenceNumber++;
				return _sequenceNumber;
			}
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

	}
}
