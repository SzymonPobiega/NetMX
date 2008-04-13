using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;

namespace NetMX
{
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
		/// <param name="type">The CLR type of the described notifications.</param>
		/// <param name="description">A human readable description of the data.</param>
		public MBeanNotificationInfo(string[] notifTypes, Type type, string description)
			: base(type.AssemblyQualifiedName, description)
		{
			_notifTypes = Array.AsReadOnly<string>(notifTypes);
		}
		/// <summary>
		/// Constructs <see cref="NetMX.MBeanNotificationInfo"/> object.
		/// </summary>
		/// <param name="notifTypes">The array of strings (in dot notation) containing the notification types that 
		/// the MBean may emit.</param>
		/// <param name="notificationType">.NET type of the notification.</param>
		public MBeanNotificationInfo(EventInfo eventInfo, Type handlerType)
			: base(handlerType.GetGenericArguments()[0].AssemblyQualifiedName, InfoUtils.GetDescrition(eventInfo, eventInfo, "MBean notification"))
		{
			MBeanNotificationAttribute attribute = (MBeanNotificationAttribute)eventInfo.GetCustomAttributes(typeof(MBeanNotificationAttribute), true)[0];
			List<string> notifTypes = new List<string>();
			notifTypes.Add(attribute.NotifType);
			_notifTypes = notifTypes.AsReadOnly();
		}
	}
}
