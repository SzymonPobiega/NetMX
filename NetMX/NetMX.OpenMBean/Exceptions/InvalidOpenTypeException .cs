#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.OpenMBean
{   
   /// <summary>
	/// This exception is thrown to indicate that the open type of an open data value is not the one expected.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class InvalidOpenTypeException : ArgumentException
   {
		private OpenType _type;
      /// <summary>
      /// Open type which caused the problem.
      /// </summary>
		public OpenType OpenType
      {
			get { return _type; }
      }      
      /// <summary>
      /// Creates new InvalidOpenTypeException object.
      /// </summary>
		/// <param name="type">Open type which caused the problem.</param>
      public InvalidOpenTypeException(OpenType type) 
			: base()
      {
			_type = type;
      }
		private InvalidOpenTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Type specificType = Type.GetType(info.GetString("typeType"), true);
			_type = (OpenType) info.GetValue("type", specificType);			
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
			info.AddValue("typeType", _type.GetType().AssemblyQualifiedName);
			info.AddValue("type", _type);
      }
   }
}
