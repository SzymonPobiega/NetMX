#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX.Timer
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class NotificationNotFoundException : OperationsException
	{
	   private readonly int _notificationId;
		/// <summary>
		/// Gets timer notification identifier of missing notification.
		/// </summary>
		public int NotificationId
		{
			get { return _notificationId; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
      /// <param name="notificationId">Timer notification identifier of missing notification.</param>
      public NotificationNotFoundException(int notificationId)
			: base(string.Format(CultureInfo.CurrentCulture, "Timer notification of id \"{0}\" does not exist in this Timer MBean.", notificationId))
		{
         _notificationId = notificationId;
		}

      private NotificationNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
         _notificationId = info.GetInt32("notificationId");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
         info.AddValue("notificationId", _notificationId);
		}
	}
}
