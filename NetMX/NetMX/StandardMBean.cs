#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#endregion

namespace NetMX
{
	public sealed class StandardMBean : IDynamicMBean, IMBeanRegistration, INotficationEmitter
	{
		#region MEMBERS
        private ObjectName _objectName;
		private MBeanInfo _info;
		private object _impl;
        private IMBeanRegistration _registration;
		private Type _implType;        
        private NotificationEmitterSupport _notificationSupport;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public StandardMBean(object impl, Type intfType)
		{
			_info = CreateMBeanInfo(impl, intfType);
			_impl = impl;
			_implType = impl.GetType();
            _registration = impl as IMBeanRegistration;
            AttachNotifications(intfType);
		}
		#endregion

		#region IDynamicMBean Members
		public MBeanInfo GetMBeanInfo()
		{
			return _info;
		}

		public object GetAttribute(string attributeName)
		{
			PropertyInfo propInfo = FindAttribute(attributeName);
			return propInfo.GetValue(_impl, new object[] {});
		}

		public void SetAttribute(string attributeName, object value)
		{
			PropertyInfo propInfo = FindAttribute(attributeName);
			propInfo.SetValue(_impl, value, new object[] {});
		}

		public object Invoke(string operationName, object[] arguments)
		{
			MethodInfo methInfo = FindOperation(operationName);
			return methInfo.Invoke(_impl, arguments);
		}
		#endregion

		#region UTILITY
		private PropertyInfo FindAttribute(string attributeName)
		{
			PropertyInfo propInfo = _implType.GetProperty(attributeName, BindingFlags.Public | BindingFlags.Instance);
			if (propInfo != null)
			{
				return propInfo;
			}
			throw new AttributeNotFoundException(attributeName, _objectName.ToString(), _info.ClassName);
		}
		private MethodInfo FindOperation(string operationName)
		{
			MethodInfo methInfo = _implType.GetMethod(operationName, BindingFlags.Public | BindingFlags.Instance);
			if (methInfo != null)
			{
				return methInfo;
			}
			throw new OperationNotFoundException(operationName, _objectName.ToString(), _info.ClassName);
		}
        private void AttachNotifications(Type intfType)
        {
            foreach (EventInfo eventInfo in intfType.GetEvents())
            {
                if (eventInfo.IsDefined(typeof(MBeanNotificationAttribute), true))
                {
                    Type handlerType = eventInfo.GetAddMethod().GetParameters()[0].ParameterType;
                    if (handlerType.GetGenericTypeDefinition() == typeof(EventHandler<>) &&
                        typeof(Notification).IsAssignableFrom(handlerType.GetGenericArguments()[0]))
                    {
                        Delegate del = Delegate.CreateDelegate(handlerType, this, "HandleNotification");
                        eventInfo.AddEventHandler(_impl, del);
                    }
                }
            }
        }
        private void HandleNotification(object sender, Notification args)
        {            
            _notificationSupport.SendNotification(args);
        }
		private static MBeanInfo CreateMBeanInfo(object impl, Type intfType)
		{
			List<MBeanOperationInfo> operations = new List<MBeanOperationInfo>();
			List<MBeanAttributeInfo> attributes = new List<MBeanAttributeInfo>();
            List<MBeanNotificationInfo> notifications = new List<MBeanNotificationInfo>();

			foreach (PropertyInfo propInfo in intfType.GetProperties())
			{
				attributes.Add(new MBeanAttributeInfo(propInfo));
			}            
            foreach (EventInfo eventInfo in intfType.GetEvents())
            {
                EventHandler e;                
                if (eventInfo.IsDefined(typeof(MBeanNotificationAttribute), true))
                {
                    Type handlerType = eventInfo.GetAddMethod().GetParameters()[0].ParameterType;
                    if (handlerType.GetGenericTypeDefinition() == typeof(EventHandler<>) &&
                        typeof(Notification).IsAssignableFrom(handlerType.GetGenericArguments()[0]))
                    {
                        notifications.Add(new MBeanNotificationInfo(eventInfo, handlerType));
                    }
                    else
                    {
                        throw new NotCompliantMBeanException(impl.GetType().AssemblyQualifiedName);
                    }
                }
            }
			foreach (MethodInfo methInfo in intfType.GetMethods())
			{
				if (!methInfo.IsSpecialName)
				{					
					operations.Add(new MBeanOperationInfo(methInfo, OperationImpact.Unknown));
				}
			}
			MBeanInfo info = new MBeanInfo(impl.GetType(), attributes, operations, notifications);
			return info;
		}		
		#endregion

        #region IMBeanRegistration Members
        public void PostDeregister()
        {
            if (_registration != null)
            {
                _registration.PostDeregister();
            }
        }
        public void PostRegister(bool registrationDone)
        {
            if (_registration != null)
            {
                _registration.PostRegister(registrationDone);
            }
        }
        public void PreDeregister()
        {
            if (_registration != null)
            {
                _registration.PreDeregister();
            }
        }
        public ObjectName PreRegister(IMBeanServer server, ObjectName name)
        {            
            ObjectName newName = name;
            if (_registration != null)
            {
                newName = _registration.PreRegister(server, name);
            }
            _objectName = newName;
            _notificationSupport = new NotificationEmitterSupport();
            _notificationSupport.Initialize(newName.ToString(), _info.Notifications);
            return newName;
        }
        #endregion

        #region INotficationEmitter Members
        public void AddNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            _notificationSupport.AddNotificationListener(callback, filterCallback, handback);
        }
        public void RemoveNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
        {
            _notificationSupport.RemoveNotificationListener(callback, filterCallback, handback);
        }
        public void RemoveNotificationListener(NotificationCallback callback)
        {
            _notificationSupport.RemoveNotificationListener(callback);
        }
        public IList<MBeanNotificationInfo> NotificationInfo
        {
            get { return _notificationSupport.NotificationInfo; }
        }
        #endregion
    }
}
