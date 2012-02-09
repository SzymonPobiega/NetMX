#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NetMX.Server.InternalInfo;

#endregion

namespace NetMX.Server
{
   public sealed class StandardMBean : IDynamicMBean, IMBeanRegistration, INotificationEmitter, INotificationListener
   {
      #region MEMBERS
      private ObjectName _objectName;
      private readonly MBeanInfo _info;
      private readonly MBeanInternalInfo _internalInfo;
      private readonly object _impl;
      private readonly IMBeanRegistration _registration;
      private readonly INotificationListener _notifListener;
      private NotificationEmitterSupport _notificationSupport;
      private INotificationEmitter _notifEmitter;
      #endregion

      #region CONSTRUCTOR
      public StandardMBean(object impl, Type intfType)
      {
         _internalInfo = MBeanInternalInfo.GetCached(intfType);
         _info = _internalInfo.MBeanInfo;//CreateMBeanInfo(impl, intfType);
         _impl = impl;
         _registration = impl as IMBeanRegistration;
         _notifListener = impl as INotificationListener;
         _notifEmitter = impl as INotificationEmitter;
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
      private void AttachNotifications()
      {

         foreach (MBeanInternalNotificationInfo notifInfo in _internalInfo.Notifications)
         {
            if (typeof (Notification).IsAssignableFrom(notifInfo.HandlerGenericArgument))
            {
               Delegate del = Delegate.CreateDelegate(notifInfo.HandlerType, this, "HandleNotification");
               notifInfo.EventInfo.AddEventHandler(_impl, del);
            }
            else if (typeof (NotificationEventArgs).IsAssignableFrom(notifInfo.HandlerGenericArgument))
            {
               SimpleNotificationHandler handler = new SimpleNotificationHandler(_notificationSupport,
                                                                                 notifInfo.NotificationInfo.
                                                                                    NotifTypes[0]);
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
         if (_notifEmitter == null)
         {
            _notificationSupport = new NotificationEmitterSupport();
            _notificationSupport.Initialize(newName.ToString(), _info.Notifications);
            _notifEmitter = _notificationSupport;
            AttachNotifications();
         }
         return newName;
      }
      #endregion

      #region INotficationEmitter Members
      public void AddNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         _notifEmitter.AddNotificationListener(callback, filterCallback, handback);
      }
      public void RemoveNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback)
      {
         _notifEmitter.RemoveNotificationListener(callback, filterCallback, handback);
      }
      public void RemoveNotificationListener(NotificationCallback callback)
      {
         _notifEmitter.RemoveNotificationListener(callback);
      }
      public IList<MBeanNotificationInfo> NotificationInfo
      {
         get { return _notifEmitter.NotificationInfo; }
      }
      #endregion

      #region INotificationListener Members
      public void HandleNotification(Notification notification, object handback)
      {
         if (_notifListener != null)
         {
            _notifListener.HandleNotification(notification, handback);
         }
         else
         {
            throw new OperationsException(string.Format("Bean \"{0}\" is not a notification listener.", _objectName));
         }
      }
      #endregion

      #region Utility class
      private class SimpleNotificationHandler
      {
         private readonly string _notifType;
         private readonly NotificationEmitterSupport _support;

         public SimpleNotificationHandler(NotificationEmitterSupport support, string notifType)
         {
            _support = support;
            _notifType = notifType;				 
         }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]//U¿ywane przez AttachNotifications
//Used via reflection
// ReSharper disable UnusedMember.Local 
         private void HandleSimpleNotification(object sender, NotificationEventArgs args)
// ReSharper restore UnusedMember.Local
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