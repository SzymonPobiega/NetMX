#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NetMX.Default.InternalInfo;
#endregion

namespace NetMX.Default
{
	public sealed class StandardMBean : IDynamicMBean, IMBeanRegistration, INotficationEmitter
	{
		#region MEMBERS
		private ObjectName _objectName;
		private MBeanInfo _info;
		private MBeanInternalInfo _internalInfo;
		private object _impl;
		private IMBeanRegistration _registration;
		private Type _implType;
		private Type _intfType;
		private NotificationEmitterSupport _notificationSupport;
		#endregion

		#region PROPERTIES
		#endregion

		#region CONSTRUCTOR
		public StandardMBean(object impl, Type intfType)
		{
			_internalInfo = MBeanInternalInfo.GetCached(intfType);
			_info = _internalInfo.MBeanInfo;//CreateMBeanInfo(impl, intfType);
			_impl = impl;
			_implType = impl.GetType();
			_intfType = intfType;
			_registration = impl as IMBeanRegistration;			
		}
		#endregion

		#region IDynamicMBean Members
		public MBeanInfo GetMBeanInfo()
		{
			return _internalInfo.MBeanInfo;
		}

		public object GetAttribute(string attributeName)
		{
			PropertyInfo propInfo = FindAttribute(attributeName);
			return propInfo.GetValue(_impl, new object[] { });
		}

		public void SetAttribute(string attributeName, object value)
		{
			PropertyInfo propInfo = FindAttribute(attributeName);
			propInfo.SetValue(_impl, value, new object[] { });
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
			MBeanInternalAttributeInfo attributeInfo;
			if (_internalInfo.Attributes.TryGetValue(attributeName, out attributeInfo))
			{				
				return attributeInfo.Property;
			}			
			throw new AttributeNotFoundException(attributeName, _objectName.ToString(), _info.ClassName);
		}
		private MethodInfo FindOperation(string operationName)
		{
			MBeanInternalOperationInfo operationInfo;
			if (_internalInfo.Operations.TryGetValue(operationName, out operationInfo))
			{
				return operationInfo.MethodInfo;
			}			
			throw new OperationNotFoundException(operationName, _objectName.ToString(), _info.ClassName);
		}
		private void AttachNotifications(Type intfType)
		{
			foreach (MBeanInternalNotificationInfo notifInfo in _internalInfo.Notifications)
			{
				if (typeof(Notification).IsAssignableFrom(notifInfo.HandlerGenericArgument))
				{
					Delegate del = Delegate.CreateDelegate(notifInfo.HandlerType, this, "HandleNotification");
					notifInfo.EventInfo.AddEventHandler(_impl, del);
				}
				else if (typeof(NotificationEventArgs).IsAssignableFrom(notifInfo.HandlerGenericArgument))
				{
					SimpleNotificationHandler handler = new SimpleNotificationHandler(_notificationSupport, notifInfo.NotificationInfo.NotifTypes[0]);
					Delegate del = Delegate.CreateDelegate(notifInfo.HandlerType, handler, "HandleSimpleNotification");
					notifInfo.EventInfo.AddEventHandler(_impl, del);
				}
			}			
		}		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]//U¿ywane przez AttachNotifications
		private void HandleNotification(object sender, Notification args)
		{
			_notificationSupport.SendNotification(args);
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
			AttachNotifications(_intfType);
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

		#region Utility class
		private class SimpleNotificationHandler
		{
			private string _notifType;
			private NotificationEmitterSupport _support;

			public SimpleNotificationHandler(NotificationEmitterSupport support, string notifType)
			{
				_support = support;
				_notifType = notifType;				 
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]//U¿ywane przez AttachNotifications
			private void HandleSimpleNotification(object sender, NotificationEventArgs args)
			{
				if (sender == null)
				{
					throw new ArgumentNullException("sender");
				}
				if (args == null)
				{
					throw new ArgumentNullException("args");
				}
			   Notification notification = args.CreateNotification(_notifType, sender);
				_support.SendNotification(notification);
			}
		}
		#endregion
	}
}
