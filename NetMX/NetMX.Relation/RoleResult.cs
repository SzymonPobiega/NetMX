#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// Represents the result of a multiple access to several roles of a relation (either for reading or writing)
   /// </summary>
   /// <remarks>
   /// This class is immutable.
   /// </remarks>
   [Serializable]
   public sealed class RoleResult
   {
      #region PROPERTIES
      private ReadOnlyCollection<Role> _roles;
      /// <summary>
      /// Gets list of roles successfully accessed.
      /// </summary>
      public IList<Role> Roles
      {
         get { return _roles; }
      }
      private ReadOnlyCollection<RoleUnresolved> _unresolvedRoles;
      /// <summary>
      /// Gets list of roles unsuccessfully accessed.
      /// </summary>
      public IList<RoleUnresolved> UnresolvedRoles
      {
         get { return _unresolvedRoles; }
      }
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates new RoleResult object. Copies elements of provided enumerations to internal read-only 
      /// collections.
      /// </summary>
      /// <param name="roles">Roles successfully accessed.</param>
      /// <param name="unresolvedRoles">Roles unsuccessfully accessed.</param>
      public RoleResult(IEnumerable<Role> roles, IEnumerable<RoleUnresolved> unresolvedRoles)
      {
         if (roles != null)
         {
            _roles = new List<Role>(roles).AsReadOnly();
         }
         else
         {
            _roles = new List<Role>().AsReadOnly();
         }
         if (unresolvedRoles != null)
         {
            _unresolvedRoles = new List<RoleUnresolved>(unresolvedRoles).AsReadOnly();
         }
         else
         {
            _unresolvedRoles = new List<RoleUnresolved>().AsReadOnly();
         }
      }
      #endregion
   }
}
