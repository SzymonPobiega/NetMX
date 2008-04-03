using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Relation
{
    /// <summary>
    /// This enum describes the various problems which can be encountered when accessing a role.
    /// </summary>
    public enum RoleStatus
    {
        /// <summary>
        /// Problem type when trying to set a role value with less ObjectNames than the minimum expected cardinality.
        /// </summary>
        LessThanMinRoleDegree,
        /// <summary>
        /// Problem type when trying to set a role value with more ObjectNames than the maximum expected cardinality.
        /// </summary>
        MoreThanMaxRoleDegree,
        /// <summary>
        /// Problem type when trying to access an unknown role.
        /// </summary>
        NoRoleWithName,
        /// <summary>
        /// Problem type when trying to set a role value including the ObjectName of a MBean not registered 
        /// in the MBean Server.
        /// </summary>
        RefMBeanNotRegistered,
        /// <summary>
        /// Problem type when trying to set a role value including the ObjectName of a MBean not of the class 
        /// expected for that role.
        /// </summary>
        RefMBeanOfIncorrectClass,
        /// <summary>
        /// Problem type when trying to read a non-readable attribute.
        /// </summary>
        RoleNotReadable,
        /// <summary>
        /// Problem type when trying to update a non-writable attribute.
        /// </summary>
        RoleNotWritable
    }
}
