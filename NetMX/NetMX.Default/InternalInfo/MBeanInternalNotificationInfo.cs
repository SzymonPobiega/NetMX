using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalNotificationInfo
	{
		#region PROPERTIES
		private EventInfo _eventInfo;
		public EventInfo EventInfo
		{
			get { return _eventInfo; }
		}
		private Type _handlerGenericArgument;
		public Type HandlerGenericArgument
		{
			get { return _handlerGenericArgument; }
		}
		private Type _handlerType;
		public Type HandlerType
		{
			get { return _handlerType; }
		}
		private MBeanNotificationInfo _notifInfo;
		public MBeanNotificationInfo NotificationInfo
		{
			get { return _notifInfo; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanInternalNotificationInfo(MBeanNotificationInfo notifInfo, EventInfo eventInfo, Type handlerType, Type handlerGenericArgument)
		{
			_notifInfo = notifInfo;
			_eventInfo = eventInfo;
			_handlerType = handlerType;
			_handlerGenericArgument = handlerGenericArgument;
		}
		#endregion
	}
}
