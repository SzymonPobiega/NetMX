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
		private readonly OpenType _type;
      /// <summary>
      /// Open type which caused the problem.
      /// </summary>
		public OpenType OpenType
      {
			get { return _type; }
      }
      /// <summary>
      /// Value which does not conform to open type specification.
      /// </summary>
      private readonly object _value;
      public object Value
      {
         get { return _value; }
      }
      /// <summary>
      /// Creates new InvalidOpenTypeException object.
      /// </summary>
		/// <param name="type">Open type which caused the problem.</param>
      /// <param name="value">Value which does not conform to open type specification.</param>
      public InvalidOpenTypeException(OpenType type, object value) 
			: base()
      {
			_type = type;
         _value = value;
      }
		private InvalidOpenTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Type specificType = Type.GetType(info.GetString("typeType"), true);
         Type valueType = Type.GetType(info.GetString("valueType"), true);
			_type = (OpenType) info.GetValue("type", specificType);
		   _value = info.GetValue("value", valueType);
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
			info.AddValue("typeType", _type.GetType().AssemblyQualifiedName);
			info.AddValue("type", _type);
         info.AddValue("valueType", _value.GetType().AssemblyQualifiedName);
         info.AddValue("value", _value);
      }
   }
}
