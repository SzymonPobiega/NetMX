#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{   
   /// <summary>
    /// This exception is raised when relation id provided for a relation is already used.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class InvalidRelationIdException : RelationException
   {
      private string _relationId;
      /// <summary>
      /// Relation Id which caused the problem.
      /// </summary>
      public string RelationTypeName
      {
          get { return _relationId; }
      }      
      /// <summary>
      /// Creates new InvalidRelationIdException object.
      /// </summary>
      /// <param name="role"></param>
      public InvalidRelationIdException(string relationId) : base() 
      {
          _relationId = relationId;
      }
       private InvalidRelationIdException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
            _relationId = info.GetString("relationId");
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("relationId", _relationId);
      }
   }
}
