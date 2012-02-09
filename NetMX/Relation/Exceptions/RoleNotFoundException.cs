#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{   
   /// <summary>
   /// This exception is raised when a role in a relation does not exist, or is not readable, or is not settable.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class RoleNotFoundException : RelationException
   {
      private string _role;
      /// <summary>
      /// Role which caused the problem.
      /// </summary>
      public string Role
      {
         get { return _role; }
      }      
      /// <summary>
      /// Creates new RoleNotFoundException object.
      /// </summary>
      /// <param name="role"></param>
      public RoleNotFoundException(string role) : base() 
      {
         _role = role;
      }
      private RoleNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
         _role = info.GetString("role");
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("role", _role);
      }
   }
}
