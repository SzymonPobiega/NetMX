#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{   
   /// <summary>
   /// This exception is raised when there is no relation for a given relation id in a Relation Service.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class RelationNotFoundException : RelationException
   {
      private string _relationId;
      /// <summary>
      /// ID of relation which has not been found.
      /// </summary>
      public string RelationId
      {
         get { return _relationId; }
      }      
      /// <summary>
      /// Creates new RelationNotFoundException object.
      /// </summary>
      /// <param name="relationId">ID of relation which has not been found.</param>
      public RelationNotFoundException(string relationId) : base() 
      {
         _relationId = _relationId;
      }
      private RelationNotFoundException(SerializationInfo info, StreamingContext context)
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
