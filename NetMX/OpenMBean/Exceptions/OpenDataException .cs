#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.OpenMBean
{   
   /// <summary>
	/// This checked exception is thrown when an open type, an open data or an open MBean metadata info instance 
	/// could not be constructed because one or more validity constraints were not met.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class OpenDataException : NetMXException
   {        
      /// <summary>
		/// Creates new OpenDataException object.
      /// </summary>
		/// <param name="key">Key which caused the problem.</param>
      public OpenDataException(string message) 
			: base(message)
      {
      }
		private OpenDataException(SerializationInfo info, StreamingContext context)
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
