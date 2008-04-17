#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Relation
{
   public class RelationSupport : IRelation, RelationSupportMBean, IMBeanRegistration
   {
      #region MEMBERS
      private readonly string _relationId;
      private ObjectName _relationServiceName;
      private IMBeanServer _relationServiceMBeanServer;
      private RelationServiceMBean _relationService;
      private readonly string _relationTypeName;
      private readonly Dictionary<string, Role> _roles;
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates new RelationSupport object.
      /// 
      /// This constructor has to be used when the user relation MBean implements the interfaces expected 
      /// to be supported by a relation by delegating to a RelationSupport object.
      /// 
      /// This object needs to know the Relation Service expected to handle the relation. So it has to know 
      /// the MBean Server where the Relation Service is registered.
      /// 
      /// According to a limitation, a relation MBean must be registered in the same MBean Server as the 
      /// Relation Service expected to handle it. So the user relation MBean has to be created and 
      /// registered, and then the wrapped RelationSupport object can be created with identified MBean Server.
      /// 
      /// Nothing is done at the Relation Service level, i.e. the RelationSupport object is not added, 
      /// and no check if the provided values are correct. The object is always created, EXCEPT if:
      /// <list type="bullet">
      /// <item>one required parameter is not provided</item>
      /// <item>the same name is used for two roles.</item>
      /// </list>
      /// To be handled as a relation, the object has then to be added in the Relation Service using the Relation 
      /// Service method <see cref="RelationServiceMBean.AddRelation"/>().      
      /// </summary>
      /// <param name="relationId">Relation identifier, to identify the relation in the Relation Service. Expected to be unique in the given Relation Service.</param>
      /// <param name="relationServiceName">ObjectName of the Relation Service where the relation will be registered.
      /// It is required as this is the Relation Service that is aware of the definition of the relation type 
      /// of given relation, so that will be able to check update operations (set).
      /// </param>
      /// <param name="relationServiceMBeanServer">MBean Server where the wrapping MBean is or will be registered.
      /// Expected to be the MBean Server where the Relation Service is or will be registered.
      /// </param>
      /// <param name="relationType">Name of relation type. Expected to have been created in given Relation Service.</param>
      /// <param name="roles">Roles (Role objects) to initialised the relation. Can be null. Expected to conform to relation info in associated relation type.</param>
      public RelationSupport(string relationId, ObjectName relationServiceName, IMBeanServer relationServiceMBeanServer,
         string relationType, IEnumerable<Role> roles)
      {
         _relationId = relationId;
         if (relationServiceMBeanServer == null || relationServiceName.Domain != "")
         {
            _relationServiceName = relationServiceName;
         }
         else
         {
            _relationServiceName = new ObjectName(relationServiceMBeanServer.GetDefaultDomain(), relationServiceName.KeyPropertyList);
         }
         _relationServiceMBeanServer = relationServiceMBeanServer;
         _relationService = NetMX.NewMBeanProxy<RelationServiceMBean>(_relationServiceMBeanServer, _relationServiceName);
         _relationTypeName = relationType;
         _roles = new Dictionary<string, Role>();
         if (roles != null)
         {
            foreach (Role role in roles)
            {
               if (!_roles.ContainsKey(role.Name))
               {
                  _roles[role.Name] = role;
               }
               else
               {
                  throw new InvalidRoleValueException();
               }
            }
         }
      }
      /// <summary>
      /// Creates new RelationSupport object.
      /// 
      /// This constructor has to be used when the RelationSupport object will be registered as a MBean by the 
      /// user, or when creating a user relation MBean those class extends RelationSupport.
      /// 
      /// Nothing is done at the Relation Service level, i.e. the RelationSupport object is not added, 
      /// and no check if the provided values are correct. The object is always created, EXCEPT if:
      /// <list type="bullet">
      /// <item>one mandatory parameter is not provided</item>
      /// <item>the same name is used for two roles.</item>
      /// </list>
      /// To be handled as a relation, the object has then to be added in the Relation Service using the Relation 
      /// Service method <see cref="RelationServiceMBean.AddRelation"/>().      
      /// </summary>
      /// <param name="relationId">Relation identifier, to identify the relation in the Relation Service. Expected to be unique in the given Relation Service.</param>
      /// <param name="relationServiceName">ObjectName of the Relation Service where the relation will be registered.
      /// It is required as this is the Relation Service that is aware of the definition of the relation type 
      /// of given relation, so that will be able to check update operations (set).
      /// </param>      
      /// <param name="relationType">Name of relation type. Expected to have been created in given Relation Service.</param>
      /// <param name="roles">Roles (Role objects) to initialised the relation. Can be null. Expected to conform to relation info in associated relation type.</param>
      public RelationSupport(string relationId, ObjectName relationServiceName,
         string relationType, params Role[] roles)
         : this(relationId, relationServiceName, null, relationType, roles)
      {
      }
      /// <summary>
      /// Creates new RelationSupport object.
      /// 
      /// This constructor has to be used when the RelationSupport object will be registered as a MBean by the 
      /// user, or when creating a user relation MBean those class extends RelationSupport.
      /// 
      /// Nothing is done at the Relation Service level, i.e. the RelationSupport object is not added, 
      /// and no check if the provided values are correct. The object is always created, EXCEPT if:
      /// <list type="bullet">
      /// <item>one mandatory parameter is not provided</item>
      /// <item>the same name is used for two roles.</item>
      /// </list>
      /// To be handled as a relation, the object has then to be added in the Relation Service using the Relation 
      /// Service method <see cref="RelationServiceMBean.AddRelation"/>().      
      /// </summary>
      /// <param name="relationId">Relation identifier, to identify the relation in the Relation Service. Expected to be unique in the given Relation Service.</param>
      /// <param name="relationServiceName">ObjectName of the Relation Service where the relation will be registered.
      /// It is required as this is the Relation Service that is aware of the definition of the relation type 
      /// of given relation, so that will be able to check update operations (set).
      /// </param>      
      /// <param name="relationType">Name of relation type. Expected to have been created in given Relation Service.</param>
      /// <param name="roles">Roles (Role objects) to initialised the relation. Can be null. Expected to conform to relation info in associated relation type.</param>
      public RelationSupport(string relationId, ObjectName relationServiceName, 
         string relationType, IEnumerable<Role> roles)
         : this(relationId, relationServiceName, null, relationType, roles)
      {             
      }
      #endregion

      #region Utility      
      private static RoleInfo FindRoleInfo(string name, IEnumerable<RoleInfo> roleInfos)
      {
         foreach (RoleInfo roleInfo in roleInfos)
         {
            if (roleInfo.Name == name)
            {
               return roleInfo;
            }
         }
         return null;
      }
      #endregion

      #region IRelation Members
      public RoleResult Roles
      {
         get 
         {             
            List<Role> roles = new List<Role>();
            List<RoleUnresolved> unresolvedRoles = new List<RoleUnresolved>();
            IList<RoleInfo> roleInfos = _relationService.GetRoleInfos(_relationTypeName);
            foreach (RoleInfo info in roleInfos)
            {
               if (info.Readable)
               {
                  roles.Add(_roles[info.Name]);
               }
               else
               {
                  unresolvedRoles.Add(new RoleUnresolved(info.Name, null, RoleStatus.RoleNotReadable));
               }
            }
            return new RoleResult(roles, unresolvedRoles);
         }
      }
      public RoleResult RetrieveAllRoles()
      {
         return new RoleResult(_roles.Values, null);
      }
      public IDictionary<ObjectName, IList<string>> ReferencedMBeans
      {
         get 
         {
            Dictionary<ObjectName, IList<string>> result = new Dictionary<ObjectName, IList<string>>();
            foreach (string roleName in _roles.Keys)
            {
               foreach (ObjectName value in _roles[roleName].Value)
               {
                  if (result.ContainsKey(value))
                  {
                     result[value].Add(roleName);
                  }
                  else
                  {
                     List<string> roles = new List<string>();
                     roles.Add(roleName);
                     result[value] = roles;
                  }
               }
            }
            return result;
         }
      }
      public string Id
      {
         get { return _relationId; }
      }
      public ObjectName RelationServiceName
      {
         get { return _relationServiceName; }
      }
      public string RelationTypeName
      {
         get { return _relationTypeName; }
      }
      public int GetRoleCardinality(string roleName)
      {
         if (_roles.ContainsKey(roleName))
         {
            return _relationService.GetRoleCardinality(_relationId, roleName);
         }
         else
         {
            throw new RoleNotFoundException(roleName);
         }
      }
      public IList<ObjectName> this[string roleName]
      {
         get
         {
            if (roleName == null)
            {
               throw new ArgumentNullException("roleName");
            }
            Role r;
            if (_roles.TryGetValue(roleName, out r))
            {
               RoleInfo info = _relationService.GetRoleInfo(_relationTypeName, roleName);
               if (info.Readable)
               {
                  return r.Value;
               }
            }
            throw new RoleNotFoundException(roleName);            
         }
         set
         {
            if (roleName == null)
            {
               throw new ArgumentNullException("roleName");
            }
            Role r;
            if (_roles.TryGetValue(roleName, out r))
            {
               RoleInfo info = _relationService.GetRoleInfo(_relationTypeName, roleName);
               if (!info.Writable)
               {
                  throw new RoleNotFoundException(roleName);
               }
               if (!Role.ValidateRole(value, info, _relationServiceMBeanServer))
               {
                  throw new InvalidRoleValueException();
               }
               _roles[roleName] = new Role(roleName, value);
            }
            else
            {
               throw new RoleNotFoundException(roleName);   
            }
         }
      }         
      public void HandleMBeanUnregistration(ObjectName objectName, string roleName)
      {
         List<ObjectName> newRoleValue = new List<ObjectName>(this[roleName]);
         newRoleValue.Remove(objectName);
         this[roleName] = newRoleValue;
      }

      public RoleResult GetRoles(IEnumerable<string> roleNames)
      {
         List<Role> roles = new List<Role>();
         List<RoleUnresolved> unresolvedRoles = new List<RoleUnresolved>();
         IList<RoleInfo> roleInfos = _relationService.GetRoleInfos(_relationTypeName);
         foreach (string role in roleNames)
         {
            RoleInfo roleInfo = FindRoleInfo(role, roleInfos);
            if (roleInfo == null)
            {
               unresolvedRoles.Add(new RoleUnresolved(role, null, RoleStatus.NoRoleWithName));
            }
            else if (!roleInfo.Readable)
            {
               unresolvedRoles.Add(new RoleUnresolved(role, null, RoleStatus.RoleNotReadable));
            }
            else
            {
               roles.Add(_roles[role]);
            }
         }         
         return new RoleResult(roles, unresolvedRoles);
      }      
      public RoleResult SetRoles(IEnumerable<Role> roles)
      {
         List<Role> resovedRoles = new List<Role>();
         List<RoleUnresolved> unresolvedRoles = new List<RoleUnresolved>();
         IList<RoleInfo> roleInfos = _relationService.GetRoleInfos(_relationTypeName);
         foreach (Role role in roles)
         {
            RoleInfo roleInfo = FindRoleInfo(role.Name, roleInfos);
            if (roleInfo == null)
            {
               unresolvedRoles.Add(new RoleUnresolved(role.Name, role.Value, RoleStatus.NoRoleWithName));
            }
            else if (!roleInfo.Writable)
            {
               unresolvedRoles.Add(new RoleUnresolved(role.Name, role.Value, RoleStatus.RoleNotWritable));
            }
            else if (!roleInfo.CheckMaxDegree(role.Value.Count))
            {
               unresolvedRoles.Add(new RoleUnresolved(role.Name, role.Value, RoleStatus.MoreThanMaxRoleDegree));               
            }
            else if (!roleInfo.CheckMinDegree(role.Value.Count))
            {
               unresolvedRoles.Add(new RoleUnresolved(role.Name, role.Value, RoleStatus.LessThanMinRoleDegree));
            }
            else
            {
               _roles[role.Name] = role;
               resovedRoles.Add(role);
            }
         }
         return new RoleResult(resovedRoles, unresolvedRoles);
      }
      #endregion

      #region RelationSupportMBean Members
      public bool InRelationService
      {
         get { throw new Exception("The method or operation is not implemented."); }
      }

      public void SetRelationServiceManagementFlag(bool value)
      {
         throw new Exception("The method or operation is not implemented.");
      }
      #endregion

      #region IMBeanRegistration Members
      public void PostDeregister()
      {
      }
      public void PostRegister(bool registrationDone)
      {
      }
      public void PreDeregister()
      {
      }
      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         _relationServiceMBeanServer = server;
         if (_relationServiceName.Domain == "")
         {
            _relationServiceName = new ObjectName(server.GetDefaultDomain(), _relationServiceName.KeyPropertyList);
         }
         _relationService = NetMX.NewMBeanProxy<RelationServiceMBean>(_relationServiceMBeanServer, _relationServiceName);
         return name;
      }
      #endregion
   }
}
