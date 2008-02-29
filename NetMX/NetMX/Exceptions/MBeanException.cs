#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	[Serializable]
	public class MBeanException : NetMXException
	{
		public MBeanException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
