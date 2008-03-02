#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote
{
	public class NotificationBuffer
	{
		#region CONST
		/// <summary>
		/// Default size of notification buffer.
		/// </summary>
		public const int DefaultSize = 100;
		#endregion

		#region MEMBERS
		private LinkedList<TargetedNotification> _notifications = new LinkedList<TargetedNotification>();
		private int _maxSize = 0;		
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public NotificationBuffer(int maxSize)
		{
			if (maxSize == 0)
			{
				_maxSize = DefaultSize;
			}
			else
			{
				_maxSize = maxSize;
			}
		}
		#endregion

		#region INTERFACE
		public void AddNotification(TargetedNotification notification)
		{
			lock (_notifications)
			{
				if (_notifications.Count == _maxSize)
				{
					_notifications.RemoveLast();
				}
				_notifications.AddFirst(notification);
			}
		}
		public TargetedNotification[] GetLastNotifications(int maxCount)
		{
			lock (_notifications)
			{
				if (maxCount > _maxSize)
				{
					TargetedNotification[] results = new TargetedNotification[_notifications.Count];
					_notifications.CopyTo(results, 0);
					_notifications.Clear();
					return results;
				}
				else
				{
					List<TargetedNotification> results = new List<TargetedNotification>();
					while (_notifications.Count > 0 && results.Count < maxCount)
					{						
						results.Add(_notifications.Last.Value);
						_notifications.RemoveLast();
					}
					return results.ToArray();
				}
			}
		}
		#endregion
	}
}
