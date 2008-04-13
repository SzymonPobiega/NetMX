#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
#endregion

namespace NetMX.Default.InternalInfo
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
      private Dictionary<string, MBeanInternalConstructorInfo> _constructors;
      internal Dictionary<string, MBeanInternalConstructorInfo> Constructors
      {
         get { return _constructors; }
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
         List<MBeanConstructorInfo> constructors = new List<MBeanConstructorInfo>();
			List<MBeanAttributeInfo> attributes = new List<MBeanAttributeInfo>();
			List<MBeanNotificationInfo> notifications = new List<MBeanNotificationInfo>();

			Dictionary<string, MBeanInternalOperationInfo> internalOperations = new Dictionary<string, MBeanInternalOperationInfo>();
         Dictionary<string, MBeanInternalConstructorInfo> internalConstructors = new Dictionary<string, MBeanInternalConstructorInfo>();
			Dictionary<string, MBeanInternalAttributeInfo> internalAttributes = new Dictionary<string, MBeanInternalAttributeInfo>();
			List<MBeanInternalNotificationInfo> internalNotifications = new List<MBeanInternalNotificationInfo>();

         List<Type> types = new List<Type>();
         types.Add(intfType);
         types.AddRange(intfType.GetInterfaces());
         foreach (Type t in types)
         {
            foreach (PropertyInfo propInfo in t.GetProperties())
            {
               MBeanAttributeInfo attrInfo = new MBeanAttributeInfo(propInfo);
               attributes.Add(attrInfo);
               internalAttributes.Add(attrInfo.Name, new MBeanInternalAttributeInfo(attrInfo, propInfo));
            }
            foreach (EventInfo eventInfo in t.GetEvents())
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
            foreach (ConstructorInfo methInfo in t.GetConstructors())
            {
               MBeanConstructorInfo constructorInfo = new MBeanConstructorInfo(methInfo);
               constructors.Add(constructorInfo);
               internalConstructors.Add(constructorInfo.Name, new MBeanInternalConstructorInfo(constructorInfo, methInfo));
            }
            foreach (MethodInfo methInfo in t.GetMethods())
            {
               if (!methInfo.IsSpecialName)
               {
                  MBeanOperationInfo operationInfo = new MBeanOperationInfo(methInfo);
                  operations.Add(operationInfo);
                  internalOperations.Add(operationInfo.Name, new MBeanInternalOperationInfo(operationInfo, methInfo));
               }
            }
         }
			_info = new MBeanInfo(intfType, attributes, constructors, operations, notifications);
			_attributes = internalAttributes;
         _constructors = internalConstructors;
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
}
