using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{	
	/// <summary>
	/// Represents a notification about an event. MBeans communicate with other MBeans using ntofications.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), Serializable] //Naming convention from JMX
	public class Notification : EventArgs, ICloneable
	{
		private string _message;
		/// <summary>
		/// Gets the notification message. 
		/// </summary>
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
		private long _sequenceNumber;
		/// <summary>
		/// Gets the notification sequence number.
		/// </summary>
		public long SequenceNumber
		{
			get { return _sequenceNumber; }
			set { _sequenceNumber = value; }
		}
		private DateTime _timestamp;
		/// <summary>
		/// Gets the notification creation date and time.
		/// </summary>
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}
		private string _type;
		/// <summary>
		/// Gets the notification type.
		/// </summary>
		public string Type
		{
			get { return _type; }			
		}		
		private object _userData;
		/// <summary>
		/// Gets the user data.
		/// </summary>
		public object UserData
		{
			get { return _userData; }
			set { _userData = value; }
		}
		private object _source;
		/// <summary>
		/// Gets the notification source.
		/// </summary>
		public object Source
		{
			get { return _source; }
			set { _source = value; }
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

		#region ICloneable Members
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion
	}	
}
