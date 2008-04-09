#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Represents a notification emitted by the MBean server through the MBeanServerDelegate MBean. 
   /// The MBean Server emits the following types of notifications: MBean registration, MBean de-registration.
   /// 
   /// To receive to MBeanServerNotifications, you need to be declared as listener to the
   /// <see cref="NetMX.MBeanServerDelegate"/> MBean that represents the MBeanServer. The ObjectName of the 
   /// MBeanServerDelegate is: NetMImplementation:type=MBeanServerDelegate.
   /// </summary>
   [Serializable]
   public class MBeanServerNotification : Notification
   {
      #region CONST
      public const string RegistrationNotification = "netmx.mbean.registered";
      public const string UnregistrationNotification = "netmx.mbean.unregistered";      
      #endregion      

      #region PROPERTIES
      private ObjectName _objectName;
      /// <summary>
      /// Gets the object name of the MBean that caused the notification.
      /// </summary>
      public ObjectName ObjectName
      {
         get { return _objectName; }
      }
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates new MBeanServerNotification object.
      /// </summary>
      /// <param name="type">A string denoting the type of the notification. Set it to one these values: 
      /// <see cref="NetMX.MBeanServerNotification.RegistrationNotification"/>, 
      /// <see cref="NetMX.MBeanServerNotification.UnregistrationNotification"/>.</param>
      /// <param name="source">The MBeanServerNotification object responsible for forwarding MBean server 
      /// notification.</param>
      /// <param name="sequenceNumber">A sequence number that can be used to order received notifications.</param>
      /// <param name="objectName">The object name of the MBean that caused the notification.</param>
      public MBeanServerNotification(string type, object source, long sequenceNumber, ObjectName objectName)
         : base(type, source, sequenceNumber)
      {
         if (type != RegistrationNotification && type != UnregistrationNotification)
         {
            throw new ArgumentException("Invalid notification type.", "type");
         }
         _objectName = objectName;
      }
      #endregion
   }      
}
