using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Relation
{
   /// <summary>
   /// The Relation Service is in charge of creating and deleting relation types and relations, of handling the 
   /// consistency and of providing query mechanisms.
   /// </summary>
   public interface RelationServiceMBean
   {
      /// <summary>
      /// Adds an MBean created by the user (and registered by him in the MBean Server) as a relation in the Relation Service. 
      /// To be added as a relation, the MBean must conform to the following: 
      /// <list type="bullet">
      /// <item>implement the Relation interface</item>
      /// <item>have for RelationService ObjectName the ObjectName of current Relation Service </item>
      /// <item>have a relation id that is unique and unused in current Relation Service </item>
      /// <item>have for relation type a relation type created in the Relation Service </item>
      /// <item>have roles conforming to the role info provided in the relation type</item>
      /// </list>
      /// </summary>
      /// <param name="relationObjectName">ObjectName of the relation MBean to be added.</param>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      void AddRelation(ObjectName relationObjectName);
      /// <summary>
      /// Adds given object as a relation type. The object is expected to implement the 
      /// <see cref="NetMX.Relation.IRelationType"/> interface.
      /// </summary>
      /// <param name="relationType">Relation type object.</param>
      /// <exception cref="NetMX.Relation.InvalidRelationTypeException">If there is already a relation type with that name.</exception>
      void AddRelationType(IRelationType relationType);
      /// <summary>
      /// Checks if given Role can be read in a relation of the given type.
      /// </summary>
      /// <param name="roleName">Role to be checked.</param>
      /// <param name="relationTypeName">Name of relation type.</param>
      /// <returns></returns>
      RoleStatus CheckRoleReading(string roleName, string relationTypeName);
      /// <summary>
      /// Checks if given Role can be set in a relation of given type.
      /// </summary>
      /// <param name="roleName">Role to be checked.</param>
      /// <param name="relationTypeName">Name of relation type.</param>
      /// <param name="initFlag">flag to specify that the checking is done for the initialization of a role, 
      /// write access shall not be verified.</param>
      /// <returns></returns>
      RoleStatus CheckRoleWriting(string roleName, string relationTypeName, bool initFlag);
      /// <summary>
      /// Creates a simple relation (represented by a RelationSupport object) of given relation type, and adds 
      /// it in the Relation Service.
      /// Roles are initialized according to the role list provided in parameter. The ones not initialized in 
      /// this way are set to an empty list of ObjectNames.
      /// A RelationNotification, with type RELATION_BASIC_CREATION, is sent.
      /// </summary>
      /// <param name="relationId">Relation identifier, to identify uniquely the relation inside the Relation Service</param>
      /// <param name="relationTypeName">Name of the relation type (has to be created in the Relation Service)</param>
      /// <param name="roles">Roles to initialize roles of the relation (can be null).</param>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If a value is provided for a role that does not exist in the relation type</exception>
      /// <exception cref="NetMX.Relation.InvalidRelationIdException">If relation id already used</exception>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If relation type not known in Relation Service</exception>
      /// <exception cref="NetMX.Relation.InvalidRoleValueException">If:
      /// <list type="bullet">
      /// <item>the same role name is used for two different roles</item>
      /// <item>the number of referenced MBeans in given value is less than expected minimum degree</item>
      /// <item>the number of referenced MBeans in provided value exceeds expected maximum degree</item>
      /// <item>one referenced MBean in the value is not an Object of the MBean class expected for that role</item>
      /// <item>an MBean provided for that role does not exist</item>
      /// </list>
      /// </exception>
      void CreateRelation(string relationId, string relationTypeName, IEnumerable<Role> roles);
      /// <summary>
      /// Creates a relation type (RelationTypeSupport object) with given role infos (provided by the RoleInfo 
      /// objects), and adds it in the Relation Service.
      /// </summary>
      /// <param name="relationTypeName">Name of the relation type.</param>
      /// <param name="roleInfos">Role infos</param>
      /// <exception cref="NetMX.Relation.InvalidRelationTypeException">If:
      /// <list type="bullet">
      /// <item>there is already a relation type with that name</item>
      /// <item>the same name has been used for two different role infos</item>
      /// <item>no role info provided</item>
      /// <item>one null role info provided</item>
      /// </list>
      /// </exception>
      void CreateRelationType(string relationTypeName, IEnumerable<RoleInfo> roleInfos);
      /// <summary>
      /// Retrieves the MBeans associated to given one in a relation.
      /// </summary>
      /// <param name="objectName">ObjectName of MBean.</param>
      /// <param name="relationTypeName">Can be null; if specified, only the relations of that type will be 
      /// considered in the search. Else all relation types are considered.</param>
      /// <param name="roleName">Can be null; if specified, only the relations where the MBean is referenced in 
      /// that role will be considered. Else all roles are considered.</param>
      /// <returns>A dictionary, where the keys are the ObjectNames of the MBeans associated to given MBean, 
      /// and the value is, for each key, a list of the relation ids of the relations where the key MBean is 
      /// associated to given one (as they can be associated in several different relations).</returns>
      IDictionary<ObjectName, IList<string>> FindAssociatedMBeans(ObjectName objectName, string relationTypeName, string roleName);
      /// <summary>
      /// Retrieves the relations where a given MBean is referenced.
      /// </summary>
      /// <param name="objectName">ObjectName of MBean.</param>
      /// <param name="relationTypeName">Can be null; if specified, only the relations of that type will be 
      /// considered in the search. Else all relation types are considered.</param>
      /// <param name="roleName">an be null; if specified, only the relations where the MBean is referenced in 
      /// that role will be considered. Else all roles are considered.</param>
      /// <returns>A dictionary, where the keys are the relation ids of the relations where the MBean is 
      /// referenced, and the value is, for each key, a list of role names (as an MBean can be referenced in 
      /// several roles in the same relation).</returns>
      IDictionary<string, IList<string>> FindReferencingRelations(ObjectName objectName, string relationTypeName, string roleName);
      /// <summary>
      /// Returns the relation ids for relations of the given type.
      /// </summary>
      /// <param name="relationTypeName">Relation type name.</param>
      /// <returns></returns>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If there is no relation type with that name.</exception>
      IList<string> FindRelationsOfType(string relationTypeName);
      /// <summary>
      /// Returns all the relation ids for all the relations handled by the Relation Service.
      /// </summary>
      /// <returns></returns>
      IList<string> GetAllRelationIds();
      /// <summary>
      /// Retrieves names of all known relation types.
      /// </summary>
      /// <returns></returns>
      IList<string> GetAllRelationTypeNames();
      /// <summary>
      /// Returns all roles present in the relation.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <returns>A RoleResult object, including a role list (for roles successfully retrieved) and a 
      /// unresolved role list (for roles not readable).</returns>
      RoleResult GetAllRoles(string relationId);
      /// <summary>
      /// Gets or sets the flag to indicate if when a notification is received for the unregistration of 
      /// an MBean referenced in a relation, if an immediate "purge" of the relations (look for the relations 
      /// no longer valid) has to be performed, or if that will be performed only when the <see cref="RelationServiceMBean.PurgeRelations"/> method 
      /// is explicitly called.
      /// </summary>
      bool Purge { get; set; }
      /// <summary>
      /// Retrieves MBeans referenced in the various roles of the relation.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <returns>A dictionary mapping ObjectName -> list of String (role names)</returns>
      /// <exception cref="RelationNotFoundException">If no relation for given relation id</exception>
      IDictionary<ObjectName, IList<string>> GetReferencedMBeans(string relationId);
      /// <summary>
      /// Returns name of associated relation type for given relation.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <returns>The name of the associated relation type.</returns>
      /// <exception cref="RelationNotFoundException">If no relation for given relation id</exception>
      string GetRelationTypeName(string relationId);
      /// <summary>
      /// Retrieves role value for given role name in given relation.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <param name="roleName">Name of role.</param>
      /// <returns></returns>
      /// <exception cref="RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="RelationNotFoundException">If no relation for given relation id</exception>
      /// <exception cref="RoleNotFoundException">If:
      /// <list type="bullet">
      /// <item>there is no role with given name, or</item>
      /// <item>the role is not readable.</item>
      /// </list>        
      /// </exception>
      IList<ObjectName> GetRole(string relationId, string roleName);
      /// <summary>
      /// Retrieves the number of MBeans currently referenced in the given role.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <param name="roleName">Role name.</param>
      /// <returns>The number of currently referenced MBeans in that role.</returns>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If no relation for given relation id.</exception>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If there is no role with given name.</exception>
      int GetRoleCardinality(string relationId, string roleName);
      /// <summary>
      /// Retrieves role info for given role of a given relation type.
      /// </summary>
      /// <param name="relationTypeName">Name of relation type.</param>
      /// <param name="roleInfoName">Name of role.</param>
      /// <returns>RoleInfo object.</returns>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If the relation type is not known in the Relation Service.</exception>
      /// <exception cref="NetMX.Relation.RoleInfoNotFoundException">If the role is not part of the relation type.</exception>
      RoleInfo GetRoleInfo(string relationTypeName, string roleInfoName);
      /// <summary>
      /// Retrieves list of role infos (RoleInfo objects) of a given relation type.
      /// </summary>
      /// <param name="relationTypeName">Name of relation type.</param>
      /// <returns></returns>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If the relation type is not known in the Relation Service.</exception>
      IList<RoleInfo> GetRoleInfos(string relationTypeName);
      /// <summary>
      /// Retrieves values of roles with given names in given relation.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <param name="roleNames">Names of roles.</param>
      /// <returns>A RoleResult object, including a role list (for roles successfully retrieved) and a 
      /// unresolved role list (for roles not readable).</returns>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If no relation for given relation id</exception>        
      RoleResult GetRoles(string relationId, IEnumerable<string> roleNames);
      /// <summary>
      /// Checks if there is a relation identified in Relation Service with given relation id.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <returns>True if there is a relation, false else.</returns>
      bool HasRelation(string relationId);
      /// <summary>
      /// Checks if the Relation Service is active. Current condition is that the Relation Service must be 
      /// registered in the MBean Server
      /// </summary>
      bool Active { get; }
      /// <summary>
      /// Returns the relation id associated to the given ObjectName if the MBean has been added as a 
      /// relation in the Relation Service.
      /// </summary>
      /// <param name="objectName">ObjectName of supposed relation.</param>
      /// <returns>Relation id (String) or null (if the ObjectName is not a relation handled by the Relation Service)</returns>
      string IsRelation(ObjectName objectName);
      /// <summary>
      /// If the relation is represented by an MBean (created by the user and added as a relation in the 
      /// Relation Service), returns the ObjectName of the MBean.
      /// </summary>
      /// <param name="relationId">Relation id identifying the relation.</param>
      /// <returns>ObjectName of the corresponding relation MBean, or null if the relation is not an MBean.</returns>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation associated to that id.</exception>
      ObjectName IsRelationMBean(string relationId);
      /// <summary>
      /// Purges the relations.        
      /// </summary>
      /// <remarks>
      /// Depending on the Purge flag value, this method is either called automatically when a notification is received 
      /// for the unregistration of an MBean referenced in a relation (if the flag is set to true), or not (if 
      /// the flag is set to false).
      /// In that case it is up to the user to call it to maintain the consistency of the relations. To be kept 
      /// in mind that if an MBean is unregistered and the purge not done immediately, if the ObjectName is 
      /// reused and assigned to another MBean referenced in a relation, calling manually this PurgeRelations() 
      /// method will cause trouble, as will consider the ObjectName as corresponding to the unregistered MBean, 
      /// not seeing the new one.
      /// The behavior depends on the cardinality of the role where the unregistered MBean is referenced:
      /// <list type="bullet">
      /// <item>if removing one MBean reference in the role makes its number of references less than the minimum degree, the relation has to be removed.</item>
      /// <item>if the remaining number of references after removing the MBean reference is still in the cardinality 
      /// range, keep the relation and update it calling its <see cref="IRelation.HandleMBeanUnregistration"/>() callback.</item>
      /// </list>
      /// </remarks>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      void PurgeRelations();
      /// <summary>
      /// Removes given relation from the Relation Service.
      /// A RelationNotification notification is sent, its type being:
      /// <list type="bullet">
      /// <item><see cref="NetMX.RelationNotification.RELATION_BASIC_REMOVAL"/>if the relation was only internal to the Relation Service.</item>
      /// <item><see cref="NetMX.RelationNotification.RELATION_MBEAN_REMOVAL"/>if the relation is registered as an MBean.</item>
      /// For MBeans referenced in such relation, nothing will be done.
      /// </list>
      /// </summary>
      /// <param name="relationId">Relation id of the relation to be removed.</param>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      void RemoveRelation(string relationId);
      /// <summary>
      /// Removes given relation type from Relation Service.
      /// The relation objects of that type will be removed from the Relation Service.
      /// </summary>
      /// <param name="relationTypeName">Name of the relation type to be removed.</param>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RelationTypeNotFoundException">If there is no relation type with that name.</exception>
      void RemoveRelationType(string relationTypeName);
      ///// <summary>
      ///// Sends a notification (RelationNotification) for a relation creation. The notification type is: 
      ///// <list type="bullet">
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_BASIC_CREATION"/>if the relation is an object internal to the Relation Service </item>
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_MBEAN_CREATION"/>if the relation is a MBean added as a relation. </item>
      ///// </list>
      ///// The source object is the Relation Service itself. 
      ///// It is called in Relation Service createRelation() and addRelation() methods.
      ///// </summary>
      ///// <param name="relationId">Relation identifier of the updated relation.</param>
      ///// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      //void SendRelationCreationNotification(string relationId);
      ///// <summary>
      ///// Sends a notification (RelationNotification) for a relation removal. The notification type is:
      ///// <list type="bullet">
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_BASIC_REMOVAL"/>if the relation is an object internal to the Relation Service.</item>
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_MBEAN_REMOVAL"/>if the relation is a MBean added as a relation.</item>
      ///// </list>
      ///// </summary>
      ///// <param name="relationId">Relation identifier of the updated relation.</param>
      ///// <param name="unregMBeans">ObjectNames of MBeans expected to be unregistered due to relation removal (can be null).</param>
      ///// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      //void SendRelationRemovalNotification(string relationId, IEnumerable<ObjectName> unregMBeans);
      ///// <summary>
      ///// Sends a notification (RelationNotification) for a role update in the given relation. The notification type is:
      ///// <list type="bullet">
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_BASIC_UPDATE"/>if the relation is an object internal to the Relation Service.</item>
      ///// <item><see cref="NetMX.Relation.RelationNotification.RELATION_MBEAN_UPDATE"/>if the relation is a MBean added as a relation.</item>
      ///// </list>
      ///// The source object is the Relation Service itself. 
      ///// </summary>
      ///// <remarks>
      ///// It is called in relation MBean SetRole() (for given role) and SetRoles() (for each role) methods 
      ///// (implementation provided in <see cref="NetMX.Relation.RelationSupport"/> class).
      ///// </remarks>
      ///// <param name="relationId">Relation identifier of the updated relation.</param>
      ///// <param name="newRole">New role (name and new value).</param>
      ///// <param name="oldRoleValue">Old role value.</param>
      ///// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      //void SendRoleUpdateNotification(string relationId, Role newRole, IList<ObjectName> oldRoleValue);
      /// <summary>
      /// Sets the given role in given relation.
      /// Will check the role according to its corresponding role definition provided in relation's relation type.
      /// The Relation Service will keep track of the change to keep the consistency of relations by handling referenced MBean unregistrations.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <param name="role">Role to be set.</param>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      /// <exception cref="NetMX.Relation.RoleNotFoundException">If:
      /// <list type="bullet">
      /// <item>internal relation, and</item>
      /// <item>the role does not exist or is not writable</item>
      /// </list>
      /// </exception>
      /// <exception cref="NetMX.Relation.InvalidRoleValueException">If internal relation and value provided for role is not valid:
      /// <list type="bullet">
      /// <item>the number of referenced MBeans in given value is less than expected minimum degree, or</item>
      /// <item>the number of referenced MBeans in provided value exceeds expected maximum degree.</item>
      /// </list>
      /// </exception>
      void SetRole(string relationId, Role role);
      /// <summary>
      /// Sets the given roles in given relation.
      /// Will check the role according to its corresponding role definition provided in relation's relation type.
      /// The Relation Service keeps track of the changes to keep the consistency of relations by handling referenced MBean unregistrations.
      /// </summary>
      /// <param name="relationId">Relation id.</param>
      /// <param name="roles">Roles to be set.</param>
      /// <returns>a RoleResult object, including a Role list (for roles successfully set) and a RoleUnresolved list (for roles not set).</returns>
      /// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      /// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      RoleResult SetRoles(string relationId, IEnumerable<Role> roles);
      ///// <summary>
      ///// Handles update of the Relation Service role map for the update of given role in given relation.
      ///// It is called in relation MBean SetRole() (for given role) and SetRoles() (for each role) methods 
      ///// (implementation provided in <see cref="NetMX.Relation.RelationSupport"/> class).
      ///// It is also called in Relation Service SetRole() (for given role) and SetRoles() (for each role) methods.
      ///// To allow the Relation Service to maintain the consistency (in case of MBean unregistration) and to be 
      ///// able to perform queries, this method must be called when a role is updated.
      ///// </summary>
      ///// <param name="relationId">Relation identifier of the updated relation.</param>
      ///// <param name="newRole">New role.</param>
      ///// <param name="oldRoleValue">Old role value.</param>
      ///// <exception cref="NetMX.Relation.RelationServiceNotRegisteredException">If the Relation Service is not registered in the MBean Server</exception>
      ///// <exception cref="NetMX.Relation.RelationNotFoundException">If there is no relation with that id.</exception>
      //void UpdateRoleMap(string relationId, Role newRole, IList<ObjectName> oldRoleValue);
   }
}
