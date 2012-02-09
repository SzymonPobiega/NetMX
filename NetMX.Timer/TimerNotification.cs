using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Timer
{
   /// <summary>
   /// This class provides definitions of the notifications sent by timer MBeans.
   /// It defines a timer notification identifier which allows to retrieve a timer notification from the list 
   /// of notifications of a timer MBean.
   // The timer notifications are created and handled by the timer MBean. 
   /// </summary>
   [Serializable]
   public sealed class TimerNotification : Notification
   {
      private readonly int _notificationId;
      /// <summary>
      /// Gets the identifier of this timer notification.
      /// </summary>
      public int NotificationId
      {
         get { return _notificationId; }  
      }

      /// <summary>
		/// Creates new <see cref="NetMX.Notification"/> object.
		/// </summary>
		/// <param name="type">Notification type.</param>
		/// <param name="source">Notification source.</param>
		/// <param name="sequenceNumber">Sequence number.</param>
		/// <param name="message">Message.</param>
		/// <param name="userData">Used defined data.</param>
      /// <param name="notificationId">Notification identifier.</param>
      public TimerNotification(string type, object source, long sequenceNumber, string message, object userData, int notificationId)
			: base(type, source, sequenceNumber, message, userData)
		{
         _notificationId = notificationId;
		}
   }
}
