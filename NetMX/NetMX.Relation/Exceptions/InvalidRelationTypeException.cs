#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
    /// <summary>
    /// Invalid relation type. This exception is raised when, in a relation type, there is already a relation 
    /// type with that name, or the same name has been used for two different role infos, or no role info provided, or one null role info provided.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
    public sealed class InvalidRelationTypeException : RelationException
    {
        private string _relationTypeName;
        /// <summary>
        /// Relation type name of relation which caused the problem.
        /// </summary>
        public string RelationTypeName
        {
            get { return _relationTypeName; }
        }
        /// <summary>
        /// Creates new InvalidRelationTypeException object.
        /// </summary>
        /// <param name="role"></param>
        public InvalidRelationTypeException(string relationTypeName)
            : base()
        {
            _relationTypeName = relationTypeName;
        }
        private InvalidRelationTypeException(SerializationInfo info, StreamingContext context)
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
