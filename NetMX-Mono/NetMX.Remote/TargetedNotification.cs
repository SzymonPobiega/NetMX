#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote
{
	[Serializable]
	public class TargetedNotification
	{
		#region PROPERTIES
		private Notification _notification;
		/// <summary>
		/// The emitted notification.
		/// </summary>
		public Notification Notification
		{
			get { return _notification; }
		}
		private int _listenerId;
		/// <summary>
		/// The ID of the listener to which the notification is targeted.
		/// </summary>
		public int ListenerId
		{
			get { return _listenerId; }
		}
		#endregion		

		#region CONSTRUCTOR
		/// <summary>
		/// Constructs a TargetedNotification object. The object contains a pair (Notification, Listener ID). 
		/// The Listener ID identifies the client listener to which that notification is targeted. The client 
		/// listener ID is one previously returned by the connector server in response to an AddNotificationListener 
		/// request
		/// </summary>
		/// <param name="notification">Notification emitted from the MBean server.</param>
		/// <param name="listenerId">The ID of the listener to which this notification is targeted.</param>
		public TargetedNotification(Notification notification, int listenerId)
		{			
			_notification = notification;
			_listenerId = listenerId;
		}
		#endregion
	}
}
