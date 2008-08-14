#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX.Monitor
{
   /// <summary>
   /// Exception thrown by the monitor when a monitor setting becomes invalid while the monitor is running.
   /// As the monitor attributes may change at runtime, a check is performed before each observation. If 
   /// a monitor attribute has become invalid, a monitor setting exception is thrown. 
   /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class MonitorSettingException : NetMXException
	{	   
		/// <summary>
		/// Constructor.
		/// </summary>      
      public MonitorSettingException(string message)
			: base(message)
		{         
		}

      private MonitorSettingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{         
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);         
		}
	}
}
