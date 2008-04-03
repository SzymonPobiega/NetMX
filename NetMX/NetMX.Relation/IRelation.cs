#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Relation
{
   public interface IRelation
   {
      /// <summary>
      /// Gets all roles present in the relation.
      /// </summary>
      /// <remarks>
      /// Returns a RoleResult object, including a role list (for roles successfully retrieved) and a 
      /// unresolved role list (for roles not readable).
      /// </remarks>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      RoleResult Roles { get; }
      /// <summary>
      /// Returns all roles in the relation without checking read mode.
      /// </summary>
      /// <returns></returns>
      RoleResult RetrieveAllRoles();
      /// <summary>
      /// Gets MBeans referenced in the various roles of the relation.
      /// </summary>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      IDictionary<ObjectName, IList<string>> ReferencedMBeans { get; }
      /// <summary>
      /// Gets relation identifier (used to uniquely identify the relation inside the Relation Service).
      /// </summary>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      string Id { get; }
      /// <summary>
      /// Gets ObjectName of the Relation Service handling the relation.
      /// </summary>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      ObjectName RelationServiceName { get; }
      /// <summary>
      /// Gets name of associated relation type.
      /// </summary>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      string RelationTypeName { get; }
      /// <summary>
      /// Gets the number of MBeans currently referenced in the given role.
      /// </summary>
      /// <param name="roleName">Name of role</param>
      /// <returns>The number of currently referenced MBeans in that role</returns>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If there is no role with given name or role is not readable.</exception>
      int GetRoleCardinality(string roleName);
      /// <summary>
      /// Gets or sets role value for given role name.      
      /// </summary>
      /// <remarks>Checks if the role exists and is readable according to the relation type. </remarks>
      /// <param name="roleName">Name of the role.</param>
      /// <returns></returns>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If there is no role with given name or role is not readable.</exception>
      IList<string> this[string roleName] { get; set; }
      /// <summary>
      /// Callback used by the Relation Service when a MBean referenced in a role is unregistered.
      /// The Relation Service will call this method to let the relation take action to reflect the impact of such unregistration.
      /// BEWARE. the user is not expected to call this method.
      /// </summary>
      /// <remarks>
      /// Current implementation is to set the role with its current value 
      /// (list of ObjectNames of referenced MBeans) without the unregistered one.
      /// </remarks>
      /// <param name="objectName">ObjectName of unregistered MBean.</param>
      /// <param name="roleName">Name of role where the MBean is referenced.</param>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If role does not exist in the relation or is not writable.</exception>
      /// <exception cref="NetMX.Relation.InvalidRoleValueException">If role value does not conform to the associated role info (this will never happen when called from the Relation Service).</exception>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server.</exception>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If the relation type has not been declared in the Relation Service.</exception>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If this method is called for a relation MBean not added in the Relation Service.</exception>
      void HandleMBeanUnregistration(ObjectName objectName, string roleName);
      /// <summary>
      /// Retrieves values of roles with given names.
      /// </summary>
      /// <remarks>
      /// Checks for each role if it exists and is readable according to the relation type. 
      /// Returns a RoleResult object, including a role list (for roles successfully retrieved) and a 
      /// unresolved role list (for roles not readable).
      /// </remarks>
      /// <param name="roleNames">Names of roles to be retrieved,</param>
      /// <returns></returns>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      RoleResult GetRoles(IEnumerable<string> roleNames);
      /// <summary>
      /// Sets the given roles.
      /// </summary>
      /// <remarks>
      /// Will check the role according to its corresponding role definition provided in relation's relation 
      /// type.
      /// Will send one notification (RelationNotification with type RELATION_BASIC_UPDATE or 
      /// RELATION_MBEAN_UPDATE, depending if the relation is a MBean or not) per updated role. 
      /// </remarks>
      /// <param name="roles">Roles to be set</param>
      /// <returns>A RoleResult object, including a RoleList (for roles successfully set) and a RoleUnresolvedList (for roles not set).</returns>
      RoleResult SetRoles(IEnumerable<Role> roles);
   }
}
