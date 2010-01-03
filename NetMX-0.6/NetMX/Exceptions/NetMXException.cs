#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[Serializable]
	public abstract class NetMXException : Exception
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		protected NetMXException() : base() { }
		/// <summary>
		/// Message constructor.
		/// </summary>
		/// <param name="message"></param>
		protected NetMXException(string message) : base(message) { }
		/// <summary>
		/// Wrapper constructor.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		protected NetMXException(string message, Exception inner) : base(message, inner) { }		
		protected NetMXException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
