#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Should be implemented by the dynamic MBean that wants to receive notifications. 
   /// Non-MBeans should use <see cref="NetMX.NotificationCallback"/> delegate insted of this interface to register for notifications.
   /// Not implemented: Standard MBeans can use <see cref="NetMX.NotificationHandlerAttribute"/> attribute to mark method as notification handler.
   /// </summary>
   public interface INotificationListener
   {
      /// <summary>
      /// Invoked when a JMX notification occurs. The implementation of this method should return as soon as 
      /// possible, to avoid blocking its notification broadcaster.
      /// </summary>
      /// <param name="notification">The notification.</param>
      /// <param name="handback">An opaque object which helps the listener to associate information regarding 
      /// the MBean emitter. This object is passed to the MBean during the AddListener call and resent, 
      /// without modification, to the listener. The MBean object should not use or modify the object.</param>
      void HandleNotification(Notification notification, object handback);
   }   
}
