using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
	/// <summary>
	/// Represents simlified a Notification. It is used in standard MBeans in events which should be exposed
	/// as notifications (in connection with <see cref="NetMX.MBeanNotificationAttribute"/>.
	/// </summary>
	public class NotificationEventArgs : EventArgs
	{
		private string _message;
		/// <summary>
		/// Message to put into notification.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
		private object _userData;
		/// <summary>
		/// Used-supplied data describing the notification.
		/// </summary>
		public object UserData
		{
			get { return _userData; }
		}
		/// <summary>
		/// Constructs <see cref="NetMX.NotificationEventArgs"/> object.
		/// </summary>
		/// <param name="message">Message to put into notification.</param>
		/// <param name="userData">Used-supplied data describing the notification.</param>
		public NotificationEventArgs(string message, object userData)
		{
			_message = message;
			_userData = userData;
		}

		/// <summary>
		/// Creates <see cref="NetMX.Notification"/> object (or instance of it's subclass) from data
		/// contained in this EventArgs.
		/// </summary>
		/// <param name="type">Type of the notification.</param>
		/// <param name="source">Source of the notifcation.</param>
		/// <returns></returns>
		public virtual Notification CreateNotification(string type, object source)
		{
			return new Notification(type, source, -1, Message, UserData);
		}
	}
}
