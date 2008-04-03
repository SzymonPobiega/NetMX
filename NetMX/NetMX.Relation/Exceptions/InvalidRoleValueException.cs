#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
    /// <summary>
    /// Role value is invalid. This exception is raised when, in a role, the number of referenced MBeans in 
    /// given value is less than expected minimum degree, or the number of referenced MBeans in provided value 
    /// exceeds expected maximum degree, or one referenced MBean in the value is not an Object of the MBean class 
    /// expected for that role, or an MBean provided for that role does not exist.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable] //Other constructos do not make sense.
    public sealed class InvalidRoleValueException : RelationException
    {
        /// <summary>
        /// Creates new InvalidRoleValueException object.
        /// </summary>      
        public InvalidRoleValueException()
            : base()
        {
        }
        private InvalidRoleValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
