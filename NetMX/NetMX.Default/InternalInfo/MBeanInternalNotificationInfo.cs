using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Server.InternalInfo
{
   internal sealed class MBeanInternalNotificationInfo
   {
      #region PROPERTIES
      private readonly EventInfo _eventInfo;
      public EventInfo EventInfo
      {
         get { return _eventInfo; }
      }
      private readonly Type _handlerGenericArgument;
      public Type HandlerGenericArgument
      {
         get { return _handlerGenericArgument; }
      }
      private readonly Type _handlerType;
      public Type HandlerType
      {
         get { return _handlerType; }
      }
      private readonly MBeanNotificationInfo _notifInfo;
      public MBeanNotificationInfo NotificationInfo
      {
         get { return _notifInfo; }
      }
      #endregion

      #region CONSTRUCTOR
      public MBeanInternalNotificationInfo(EventInfo eventInfo, IMBeanInfoFactory factory)
      {
         _handlerType = eventInfo.GetAddMethod().GetParameters()[0].ParameterType;
         _handlerGenericArgument = _handlerType.GetGenericArguments()[0];
         if (_handlerType.GetGenericTypeDefinition() == typeof(EventHandler<>) &&
             (typeof(Notification).IsAssignableFrom(_handlerGenericArgument)
              || typeof(NotificationEventArgs).IsAssignableFrom(_handlerGenericArgument)))
         {
            _notifInfo = factory.CreateMBeanNotificationInfo(eventInfo, _handlerType);
         }
         else
         {
            throw new NotCompliantMBeanException(eventInfo.DeclaringType.AssemblyQualifiedName);
         }       
         _eventInfo = eventInfo;         
      }
      #endregion
   }
}