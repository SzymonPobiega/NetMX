#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using NetMX.OpenMBean;

#endregion

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalInfo
	{
		#region MEMBERS		
		private static readonly Dictionary<Type, MBeanInternalInfo> _cache = new Dictionary<Type, MBeanInternalInfo>();
		private static readonly object _synchRoot = new object();
		#endregion

		#region PROPERTIES
		private readonly MBeanInfo _info;
		public MBeanInfo MBeanInfo
		{
			get { return _info; }
		}
		private readonly Dictionary<string,MBeanInternalAttributeInfo> _attributes;
		internal Dictionary<string,MBeanInternalAttributeInfo> Attributes
		{
			get { return _attributes; }
		}
      private readonly Dictionary<string, MBeanInternalConstructorInfo> _constructors;
      internal Dictionary<string, MBeanInternalConstructorInfo> Constructors
      {
         get { return _constructors; }
      }
		private readonly Dictionary<string,MBeanInternalOperationInfo> _operations;
		internal Dictionary<string,MBeanInternalOperationInfo> Operations
		{
			get { return _operations; }
		}
		private readonly List<MBeanInternalNotificationInfo> _notifications;
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
		   IMBeanInfoFactory factory;
         if (intfType.IsDefined(typeof(OpenMBeanAttribute), true))
         {
            factory = new OpenMBeanBeanInfoFactory();
         }
         else
         {
            factory = new StandardBeanInfoFactory();
         }
         foreach (Type t in types)
         {
            foreach (PropertyInfo propInfo in t.GetProperties())
            {
               MBeanInternalAttributeInfo attrInfo = new MBeanInternalAttributeInfo(propInfo, factory);
               attributes.Add(attrInfo.AttributeInfo);
               internalAttributes.Add(attrInfo.AttributeInfo.Name, attrInfo);
            }
            foreach (EventInfo eventInfo in t.GetEvents())
            {
               if (eventInfo.IsDefined(typeof(MBeanNotificationAttribute), true))
               {
                  MBeanInternalNotificationInfo notifInfo = new MBeanInternalNotificationInfo(eventInfo, factory);
                  notifications.Add(notifInfo.NotificationInfo);
                  internalNotifications.Add(notifInfo);                  
               }
            }
            foreach (ConstructorInfo methInfo in t.GetConstructors())
            {
               MBeanInternalConstructorInfo constructorInfo = new MBeanInternalConstructorInfo(methInfo, factory);
               constructors.Add(constructorInfo.ConstructorInfo);
               internalConstructors.Add(constructorInfo.ConstructorInfo.Name, constructorInfo);
            }
            foreach (MethodInfo methInfo in t.GetMethods())
            {
               if (!methInfo.IsSpecialName)
               {
                  MBeanInternalOperationInfo operationInfo = new MBeanInternalOperationInfo(methInfo, factory);
                  operations.Add(operationInfo.OperationInfo);
                  internalOperations.Add(operationInfo.OperationInfo.Name, operationInfo);
               }
            }
         }
			_info = factory.CreateMBeanInfo(intfType, attributes, constructors, operations, notifications);
			_attributes = internalAttributes;
         _constructors = internalConstructors;
			_operations = internalOperations;
			_notifications = internalNotifications;
		}	   
	   #endregion

		#region INTERFACE
      internal static MBeanInternalInfo GetCached(Type intfType)
      {
         lock (_synchRoot)
         {
            if (!_cache.ContainsKey(intfType))
            {
               MBeanInternalInfo info = new MBeanInternalInfo(intfType);
               _cache[intfType] = info;
            }
         }
         return _cache[intfType];
      }
	   #endregion
	}
}
