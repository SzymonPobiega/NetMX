#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public class MBeanException : NetMXException
	{
		public MBeanException(string message, Exception inner)
			: base(message, inner)
		{
		}
		protected MBeanException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
