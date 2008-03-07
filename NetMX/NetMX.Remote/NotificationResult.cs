using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote
{
	/// <summary>
	/// Result of a query for buffered notifications. Notifications in a notification buffer have positive, 
	/// monotonically increasing sequence numbers. The result of a notification query contains the following 
	/// elements:	
	/// <list type="bullet">
	/// <item>The sequence number of the earliest notification still in the buffer.</item>
	/// <item>The sequence number of the next notification available for querying. This will be the starting 
	/// sequence number for the next notification query.</item>
   /// <item>An array of (Notification,listenerID) pairs corresponding to the returned notifications and the 
	/// listeners they correspond to.</item>
	/// </list>
	/// <remarks>
	/// It is possible for the nextSequenceNumber to be less than the earliestSequenceNumber. This signifies that 
	/// notifications between the two might have been lost.
	/// </remarks>
	/// </summary>
	[Serializable]
	public sealed class NotificationResult
	{
		private int _earliestSequenceNumber;
		/// <summary>
		/// Gets the sequence number of the earliest notification still in the buffer.
		/// </summary>
		public int EarliestSequenceNumber
		{
			get { return _earliestSequenceNumber; }
		}
		private int _nextSequenceNumber;
		/// <summary>
		/// Gets the sequence number of the next notification available for querying.
		/// </summary>
		public int NextSequenceNumber
		{
			get { return _nextSequenceNumber; }
			set { _nextSequenceNumber = value; }
		}
		private TargetedNotification[] _targetedNotifications;
		/// <summary>
		/// Gets the notifications resulting from the query, and the listeners they correspond to.
		/// </summary>
		public TargetedNotification[] TargetedNotifications
		{
			get { return _targetedNotifications; }
		}

		/// <summary>
		/// Constructs a notification query result.
		/// </summary>
		/// <param name="earliestSequenceNumber">The sequence number of the earliest notification still in the buffer.</param>
		/// <param name="nextSequenceNumber">The sequence number of the next notification available for querying.</param>
		/// <param name="targetedNotifications">The notifications resulting from the query, and the listeners they correspond to. This array can be empty.</param>
		public NotificationResult(int earliestSequenceNumber, int nextSequenceNumber, TargetedNotification[] targetedNotifications)
		{
			_earliestSequenceNumber = earliestSequenceNumber;
			_nextSequenceNumber = nextSequenceNumber;
			_targetedNotifications = targetedNotifications;
		}
	}
}
