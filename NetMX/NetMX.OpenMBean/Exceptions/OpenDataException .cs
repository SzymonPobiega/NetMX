#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.OpenMBean
{   
   /// <summary>
	/// This exception is thrown to indicate that a method parameter which was expected to be an item name of a 
	/// composite data or a row index of a tabular data is not valid.
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
