#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.OpenMBean
{   
   /// <summary>
	/// This runtime exception is thrown to indicate that the index of a row to be added to a tabular data 
	/// instance is already used to refer to another row in this tabular data instance.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class KeyAlreadyExistsException : ArgumentException
   {
      private string _key;
      /// <summary>
      /// Key which caused the problem.
      /// </summary>
      public string Key
      {
			get { return _key; }
      }      
      /// <summary>
		/// Creates new KeyAlreadyExistsException object.
      /// </summary>
		/// <param name="key">Key which caused the problem.</param>
      public KeyAlreadyExistsException(string key) 
			: base()
      {
			_key = key;
      }
		private KeyAlreadyExistsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_key = info.GetString("key");			
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
			info.AddValue("key", _key);
      }
   }
}
