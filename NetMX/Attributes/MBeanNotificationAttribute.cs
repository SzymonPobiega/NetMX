using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{    
    [AttributeUsage(AttributeTargets.Event, AllowMultiple=false)]
    public sealed class MBeanNotificationAttribute : Attribute
    {
        private string _notifType;
        /// <summary>
        /// Notification type emited by annotated event.
        /// </summary>
        public string NotifType
        {
            get { return _notifType; }
        }
        /// <summary>
        /// Constructs <see cref="NetMX.MBeanNotificationAttribute"/> object.
        /// </summary>
        /// <param name="notifType">Notification type emited by annotated event.</param>
        public MBeanNotificationAttribute(string notifType)
        {
            _notifType = notifType;
        }
    }
}
