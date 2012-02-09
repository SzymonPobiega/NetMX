#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
    /// <summary>
    /// This exception is raised when there is no role info with given name in a given relation type.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
    public sealed class RoleInfoNotFoundException : RelationException
    {
        private string _roleName;
        /// <summary>
        /// Name of role which has not been found.
        /// </summary>
        public string RoleName
        {
            get { return _roleName; }
        }
        /// <summary>
        /// Creates new RoleInfoNotFoundException object.
        /// </summary>
        /// <param name="roleName">Name of role which has not been found.</param>
        public RoleInfoNotFoundException(string roleName)
            : base()
        {
            _roleName = roleName;
        }
        private RoleInfoNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _roleName = info.GetString("roleName");
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("roleName", _roleName);
        }
    }
}
