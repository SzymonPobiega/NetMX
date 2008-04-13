#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	/// <summary>
	/// OperationsException
	/// </summary>
	[Serializable]
	public class OperationsException : NetMXException
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public OperationsException() : base() { }
		/// <summary>
		/// Message constructor.
		/// </summary>
		/// <param name="message"></param>
		public OperationsException(string message) : base(message) { }
		/// <summary>
		/// Wrapper constructor.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public OperationsException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Serialization constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected OperationsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
