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
   public sealed class RelationTypeNotFoundException : RelationException
   {
      private string _relationTypeName;
      /// <summary>
      /// Relation type name of relation which has not been found.
      /// </summary>
      public string RelationTypeName
      {
          get { return _relationTypeName; }
      }      
      /// <summary>
      /// Creates new RelationTypeNotFoundException object.
      /// </summary>
      /// <param name="role"></param>
      public RelationTypeNotFoundException(string relationTypeName) : base() 
      {
          _relationTypeName = relationTypeName;
      }
      private RelationTypeNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
            _relationTypeName = info.GetString("relationTypeName");
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("relationTypeName", _relationTypeName);
      }
   }
}
