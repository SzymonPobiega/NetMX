using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
    /// <summary>
    /// To be invoked when a JMX notification occurs. The implementation of this callback should return as soon as possible, 
    /// to avoid blocking its notification broadcaster. 
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="handback">An opaque object which helps the listener to associate information regarding the MBean 
    /// emitter. This object is passed to the MBean during the addListener call and resent, without modification, to 
    /// the listener. The MBean object should not use or modify the object.</param>
    public delegate void NotificationCallback(Notification notification, object handback);

    /// <summary>
    /// To be invoked before sending the specified notification to the listener. 
    /// </summary>
    /// <param name="notification">The notification to be sent. </param>
    /// <returns>true if the notification has to be sent to the listener, false otherwise.</returns>
    public delegate bool NotificationFilterCallback(Notification notification);

    public interface INotficationEmitter
    {
        /// <summary>
        /// Adds a listener to this MBean. 
        /// </summary>
        /// <param name="callback">The listener object which will handle the notifications emitted by the broadcaster.</param>
        /// <param name="filterCallback">The filter object. If filter is null, no filtering will be performed before handling notifications.</param>
        /// <param name="handback">An opaque object to be sent back to the listener when a notification is emitted. 
        /// This object cannot be used by the Notification broadcaster object. It should be resent unchanged with the notification to the listener.</param>
        void AddNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
        /// <summary>
        /// Removes a listener from this MBean. The MBean must have a listener that exactly matches the given 
        /// listener, filter, and handback parameters. If there is more than one such listener, only one is removed.
        /// </summary>
        /// <remarks>
        /// The filter and handback parameters may be null if and only if they are null in a listener to be removed.
        /// </remarks>
        /// <param name="callback">A listener that was previously added to this MBean.</param>
        /// <param name="filterCallback">The filter that was specified when the listener was added.</param>
        /// <param name="handback">The handback that was specified when the listener was added.</param>
        /// <exception cref="NetMX.ListenerNotFoundException"/>
        void RemoveNotificationListener(NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
        /// <summary>
        /// Removes a listener from this MBean. If the listener has been registered with different handback objects 
        /// or notification filters, all entries corresponding to the listener will be removed.
        /// </summary>
        /// <param name="callback">A listener that was previously added to this MBean.</param>
        /// <exception cref="NetMX.ListenerNotFoundException"/>
        void RemoveNotificationListener(NotificationCallback callback);
        /// <summary>
        /// Returns an array indicating, for each notification this MBean may send, the name of the .NET class 
        /// of the notification and the notification type.
        /// </summary>
        /// <remarks>
        /// It is not illegal for the MBean to send notifications not described in this array. However, some 
        /// clients of the MBean server may depend on the array being complete for their correct functioning.        
        /// </remarks>
        IList<MBeanNotificationInfo> NotificationInfo { get; }
    }
}
