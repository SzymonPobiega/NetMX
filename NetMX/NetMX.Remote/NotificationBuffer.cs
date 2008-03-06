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
		private LinkedList<KeyValuePair<int, TargetedNotification>> _notifications = new LinkedList<KeyValuePair<int, TargetedNotification>>();
		private int _maxSize = 0;
		private int _sequenceNumber = 0;
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
				_sequenceNumber++;
				if (_notifications.Count == _maxSize)
				{
					_notifications.RemoveLast();
				}
				_notifications.AddFirst(new KeyValuePair<int, TargetedNotification>(_sequenceNumber, notification));
			}
		}
		public NotificationResult FetchNotifications(int nextSequenceNumber, int maxCount)
		{
			lock (_notifications)
			{
				int earliestSequenceNumber = 0;
				if (_notifications.Count > 0)
				{
					earliestSequenceNumber = _notifications.First.Value.Key;
				}							
				int lastSequenceNumber = nextSequenceNumber;
				List<TargetedNotification> results = new List<TargetedNotification>();
				while (_notifications.Count > 0 && results.Count < maxCount)
				{
					lastSequenceNumber = _notifications.Last.Value.Key;
					results.Add(_notifications.Last.Value.Value);
					_notifications.RemoveLast();
				}
				return new NotificationResult(earliestSequenceNumber, lastSequenceNumber+1, results.ToArray());
			}
		}
		#endregion
	}
}
