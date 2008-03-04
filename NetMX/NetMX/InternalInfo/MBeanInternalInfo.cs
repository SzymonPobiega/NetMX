#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
#endregion

namespace NetMX.InternalInfo
{
	internal sealed class MBeanInternalInfo
	{
		#region MEMBERS		
		private static Dictionary<Type, MBeanInternalInfo> _cache = new Dictionary<Type, MBeanInternalInfo>();
		private static object _synchRoot = new object();
		#endregion

		#region PROPERTIES
		private MBeanInfo _info;
		public MBeanInfo MBeanInfo
		{
			get { return _info; }
		}
		private Dictionary<string,MBeanInternalAttributeInfo> _attributes;
		internal Dictionary<string,MBeanInternalAttributeInfo> Attributes
		{
			get { return _attributes; }
		}
		private Dictionary<string,MBeanInternalOperationInfo> _operations;
		internal Dictionary<string,MBeanInternalOperationInfo> Operations
		{
			get { return _operations; }
		}
		private List<MBeanInternalNotificationInfo> _notifications;
		internal List<MBeanInternalNotificationInfo> Notifications
		{
			get { return _notifications; }
		}
		#endregion

		#region CONSTRUCTOR
		private MBeanInternalInfo(Type intfType)
		{
			List<MBeanOperationInfo> operations = new List<MBeanOperationInfo>();
			List<MBeanAttributeInfo> attributes = new List<MBeanAttributeInfo>();
			List<MBeanNotificationInfo> notifications = new List<MBeanNotificationInfo>();

			Dictionary<string, MBeanInternalOperationInfo> internalOperations = new Dictionary<string, MBeanInternalOperationInfo>();
			Dictionary<string, MBeanInternalAttributeInfo> internalAttributes = new Dictionary<string, MBeanInternalAttributeInfo>();
			List<MBeanInternalNotificationInfo> internalNotifications = new List<MBeanInternalNotificationInfo>();

			foreach (PropertyInfo propInfo in intfType.GetProperties())
			{
				MBeanAttributeInfo attrInfo = new MBeanAttributeInfo(propInfo);
				attributes.Add(attrInfo);
				internalAttributes.Add(attrInfo.Name, new MBeanInternalAttributeInfo(attrInfo, propInfo));
			}
			foreach (EventInfo eventInfo in intfType.GetEvents())
			{
				if (eventInfo.IsDefined(typeof(MBeanNotificationAttribute), true))
				{
					Type handlerType = eventInfo.GetAddMethod().GetParameters()[0].ParameterType;
					Type genericArgument = handlerType.GetGenericArguments()[0];
					if (handlerType.GetGenericTypeDefinition() == typeof(EventHandler<>))
					{
						if (typeof(Notification).IsAssignableFrom(genericArgument)
							|| typeof(NotificationEventArgs).IsAssignableFrom(genericArgument))
						{
							MBeanNotificationInfo notifInfo = new MBeanNotificationInfo(eventInfo, handlerType);
							notifications.Add(notifInfo);
							internalNotifications.Add(new MBeanInternalNotificationInfo(notifInfo, eventInfo, handlerType, genericArgument));
						}						
						else
						{
							throw new NotCompliantMBeanException(intfType.AssemblyQualifiedName);
						}
					}
				}
			}
			foreach (MethodInfo methInfo in intfType.GetMethods())
			{
				if (!methInfo.IsSpecialName)
				{
					MBeanOperationInfo operationInfo = new MBeanOperationInfo(methInfo, OperationImpact.Unknown);
					operations.Add(operationInfo);
					internalOperations.Add(operationInfo.Name, new MBeanInternalOperationInfo(operationInfo, methInfo));
				}
			}
			_info = new MBeanInfo(intfType, attributes, operations, notifications);
			_attributes = internalAttributes;
			_operations = internalOperations;
			_notifications = internalNotifications;
		}
		#endregion

		#region INTERFACE
		internal static MBeanInternalInfo GetCached(Type intfType)
		{
			if (!_cache.ContainsKey(intfType))
			{
				lock (_synchRoot)
				{
					if (!_cache.ContainsKey(intfType))
					{
						MBeanInternalInfo info = new MBeanInternalInfo(intfType);
						_cache[intfType] = info;
					}
				}
			}
			return _cache[intfType];
		}
		#endregion
	}
	internal sealed class MBeanInternalAttributeInfo
	{		
		#region PROPERTIES
		private PropertyInfo _property;
		public PropertyInfo Property
		{
			get { return _property; }
		}
		private MBeanAttributeInfo _attributeInfo;
		public MBeanAttributeInfo AttributeInfo
		{
			get { return _attributeInfo; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanInternalAttributeInfo(MBeanAttributeInfo attributeInfo, PropertyInfo property)
		{
			_attributeInfo = attributeInfo;
			_property = property;
		}
		#endregion
	}
	internal sealed class MBeanInternalOperationInfo
	{		
		#region PROPERTIES
		private MethodInfo _methodInfo;
		public MethodInfo MethodInfo
		{
			get { return _methodInfo; }
		}
		private MBeanOperationInfo _operationInfo;
		public MBeanOperationInfo OperationInfo
		{
			get { return _operationInfo; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanInternalOperationInfo(MBeanOperationInfo operationInfo, MethodInfo method)
		{
			_methodInfo = method;
			_operationInfo = operationInfo;
		}
		#endregion
	}
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
		public MBeanInternalNotificationInfo(MBeanNotificationInfo notifInfo, EventInfo eventInfo,Type handlerType, Type handlerGenericArgument)
		{
			_notifInfo = notifInfo;
			_eventInfo = eventInfo;
			_handlerType = handlerType;
			_handlerGenericArgument = handlerGenericArgument;
		}
		#endregion
	}
}
