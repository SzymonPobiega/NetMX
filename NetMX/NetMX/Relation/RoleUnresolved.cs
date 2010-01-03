#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// Represents an unresolved role: a role not retrieved from a relation due to a problem. 
   /// It provides the role name, value (if problem when trying to set the role) and an integer defining 
   /// the problem (constants defined in RoleStatus).
   /// </summary>
   /// <remarks>
   /// This class is immutable.
   /// </remarks>
   [Serializable]
   public sealed class RoleUnresolved
   {
      #region PROPERTIES
      private string _roleName;
      /// <summary>
      /// Gets role name.
      /// </summary>
      public string RoleName
      {
         get { return _roleName; }
      }
      private ReadOnlyCollection<ObjectName> _roleValue;
      /// <summary>
      /// Gets role value.
      /// </summary>
      public IList<ObjectName> RoleValue
      {
         get { return _roleValue; }
      }
      private RoleStatus _problemType;
      /// <summary>
      /// Gets problem type.
      /// </summary>
      public RoleStatus ProblemType
      {
         get { return _problemType; }
      }
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates new RoleUnresolved object. Copies referenced MBean names to its internal read-only collection.
      /// </summary>
      /// <param name="roleName">Name of the role which caused the problem.</param>
      /// <param name="roleValue">Value of the role, or null if the problem is the inability to read a role</param>
      /// <param name="problemType">Type of problem.</param>
      public RoleUnresolved(string roleName, IEnumerable<ObjectName> roleValue, RoleStatus problemType)
      {
         _roleName = roleName;
         if (roleValue != null)
         {
            _roleValue = new List<ObjectName>(roleValue).AsReadOnly();
         }
         _problemType = problemType;
      }
      #endregion
   }
}
