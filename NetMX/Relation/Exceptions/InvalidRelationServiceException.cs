#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// This exception is raised when an invalid Relation Service is provided.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
   public sealed class InvalidRelationServiceException : RelationException
   {
      private ObjectName _relationServiceName;
      /// <summary>
      /// Provided Relation Serice object name.
      /// </summary>
      public string RelationServiceName
      {
         get { return _relationServiceName; }
      }
      /// <summary>
      /// Creates new InvalidRelationServiceException object.
      /// </summary>
      /// <param name="relationServiceName">Provided Relation Serice object name.</param>
      public InvalidRelationServiceException(ObjectName relationServiceName)
         : base()
      {
         _relationServiceName = relationServiceName;
      }
      private InvalidRelationServiceException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         _relationServiceName = (ObjectName) info.GetValue("relationServiceName", typeof(ObjectName));
      }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("relationServiceName", _relationServiceName);
      }
   }
}
