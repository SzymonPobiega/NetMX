using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
    [Serializable]
    public class Notification : EventArgs
    {
        protected string _message;
        /// <summary>
        /// Gets the notification message. 
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
        private long _sequenceNumber;
        /// <summary>
        /// Gets the notification sequence number.
        /// </summary>
        public long SequenceNumber
        {
            get { return _sequenceNumber; }
        }
        private DateTime _timestamp;
        /// <summary>
        /// Gets the notification creation date and time.
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }
        private string _type;
        /// <summary>
        /// Gets the notification type.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }
        protected object _userData;
        /// <summary>
        /// Gets the user data.
        /// </summary>
        public object UserData
        {
            get { return _userData; }
        }
        private object _source;
        /// <summary>
        /// Gets the notification source.
        /// </summary>
        public object Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Creates new <see cref="NetMX.Notification"/> object.
        /// </summary>
        /// <param name="type">Notification type.</param>
        /// <param name="source">Notification source.</param>
        /// <param name="sequenceNumber">Sequence number.</param>
        public Notification(string type, object source, long sequenceNumber)
        {
            _type = type;
            _source = source;
            _sequenceNumber = sequenceNumber;
            _timestamp = DateTime.Now;
        }
        /// <summary>
        /// Creates new <see cref="NetMX.Notification"/> object.
        /// </summary>
        /// <param name="type">Notification type.</param>
        /// <param name="source">Notification source.</param>
        /// <param name="sequenceNumber">Sequence number.</param>
        /// <param name="message">Message</param>
        public Notification(string type, object source, long sequenceNumber, string message) 
            : this(type, source, sequenceNumber)
        {
            _message = message;
        }
        /// <summary>
        /// Creates new <see cref="NetMX.Notification"/> object.
        /// </summary>
        /// <param name="type">Notification type.</param>
        /// <param name="source">Notification source.</param>
        /// <param name="sequenceNumber">Sequence number.</param>
        /// <param name="message">Message.</param>
        /// <param name="userData">Used defined data.</param>
        public Notification(string type, object source, long sequenceNumber, string message, object userData)
            : this(type, source, sequenceNumber, message)
        {
            _userData = userData;
        }
    }
}
