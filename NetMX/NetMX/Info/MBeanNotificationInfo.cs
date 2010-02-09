using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
   /// <summary>
   /// Represents metadata about MBean notification.
   /// </summary>
	[Serializable]
	public class MBeanNotificationInfo : MBeanFeatureInfo
	{
		private IList<string> _notifTypes;
		/// <summary>
		/// Returns the list of strings (in dot notation) containing the notification types that the MBean may emit. 
		/// The list is immutable.
		/// </summary>
		public IList<string> NotifTypes
		{
			get { return _notifTypes; }
		}
		/// <summary>
		/// Constructs <see cref="NetMX.MBeanNotificationInfo"/> object.
		/// </summary>
		/// <param name="notifTypes">The array of strings (in dot notation) containing the notification types that 
		/// the MBean may emit.</param>
      /// <param name="notificationTypeName">The CLR type name of the described notifications.</param>
		/// <param name="description">A human readable description of the data.</param>
		public MBeanNotificationInfo(string[] notifTypes, string notificationTypeName, string description)
         : base(notificationTypeName, description)
		{
			_notifTypes = Array.AsReadOnly(notifTypes);
		}		
	}
}
