using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Relation
{
    /// <summary>
    /// The RelationType interface has to be implemented by any class expected to represent a relation type.
    /// </summary>
    public interface IRelationType
    {
        /// <summary>
        /// Gets the relation type name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the role info (RoleInfo object) for the given role info name (null if not found).
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>RoleInfo object providing role definition does not exist.</returns>
        /// <exception cref="NetMX.Relation.RoleInfoNotFoundException">If no role info with that name in relation type.</exception>
        RoleInfo this[string roleName] { get; }
        /// <summary>
        /// Gets the list of role definitions.
        /// </summary>
        IList<RoleInfo> RoleInfos { get; }
    }
}
