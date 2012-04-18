using System;
using System.Collections.Generic;

namespace NetMX.Relation
{
    /// <summary>
    /// The Relation Service is in charge of creating and deleting relation types and relations, of handling the 
    /// consistency and of providing query mechanisms.
    /// 
    /// It implements the NotificationEmitter by extending NotificationEmitterSupport to send 
    /// notifications when a relation is removed from it.
    /// 
    /// It implements the NotificationListener interface to be able to receive notifications concerning 
    /// unregistration of MBeans referenced in relation roles and of relation MBeans.
    /// 
    /// It implements the MBeanRegistration interface to be able to retrieve its ObjectName and MBean Server.
    /// </summary>   
    public class RelationService : NotificationEmitterSupport, RelationServiceMBean, IMBeanRegistration, INotificationListener
    {
        #region Const
        public const string ObjectName = ":type=RelationService";
        #endregion

        #region MEMBERS
        private bool _purge;
        private IMBeanServer _server;
        private ObjectName _ownName;
        private readonly Dictionary<string, IRelationType> _relationTypes = new Dictionary<string, IRelationType>();
        private readonly Dictionary<string, RelationWrapper> _relations = new Dictionary<string, RelationWrapper>();
        private readonly Dictionary<ObjectName, dynamic> _relationBeans = new Dictionary<ObjectName, dynamic>();
        private readonly List<ObjectName> _removedBeans = new List<ObjectName>();

        private readonly Dictionary<ObjectName, Dictionary<string, IList<string>>> _referencingRelations = new Dictionary<ObjectName, Dictionary<string, IList<string>>>();
        #endregion       

        #region RelationServiceMBean Members
        public void AddRelation(ObjectName relationObjectName)
        {
            AssertNotNull(relationObjectName, "relationObjectName");
            AssertRegistered();

            dynamic relation = _server.CreateDynamicProxy(relationObjectName);
            ValidateRelation(relation);

            _relations[relation.Id] = new RelationWrapper(relation, relationObjectName);
            _relationBeans[relationObjectName] = relation;
            foreach (Role r in relation.RetrieveAllRoles().Roles)
            {
                UpdateRoleMap(relation.Id, r, null);
            }
        }
        public void AddRelationType(IRelationType relationType)
        {
            AssertNotNull(relationType, "relationType");
            ValidateRelationType(relationType);

            _relationTypes[relationType.Name] = relationType;
        }

        public RoleStatus CheckRoleReading(string roleName, string relationTypeName)
        {
            AssertNotNull(roleName, "roleName");
            AssertNotNull(relationTypeName, "relationTypeName");

            IRelationType type;
            if (_relationTypes.TryGetValue(relationTypeName, out type))
            {
                try
                {
                    return type[roleName].Readable ? RoleStatus.RoleOk : RoleStatus.RoleNotReadable;
                }
                catch (RoleNotFoundException)
                {
                    return RoleStatus.NoRoleWithName;
                }
            }
            else
            {
                throw new RelationTypeNotFoundException(relationTypeName);
            }
        }

        public RoleStatus CheckRoleWriting(string roleName, string relationTypeName, bool initFlag)
        {
            AssertNotNull(roleName, "roleName");
            AssertNotNull(relationTypeName, "relationTypeName");

            IRelationType type;
            if (_relationTypes.TryGetValue(relationTypeName, out type))
            {
                try
                {
                    return type[roleName].Writable ? RoleStatus.RoleOk : RoleStatus.RoleNotWritable;
                }
                catch (RoleNotFoundException)
                {
                    return RoleStatus.NoRoleWithName;
                }
            }
            else
            {
                throw new RelationTypeNotFoundException(relationTypeName);
            }
        }

        public void CreateRelation(string relationId, string relationTypeName, IEnumerable<Role> roles)
        {
            AssertNotNull(relationId, "relationId");
            AssertNotNull(relationTypeName, "relationTypeName");
            AssertRegistered();

            RelationSupport relation = new RelationSupport(relationId, _ownName, _server, relationTypeName, roles);
            ValidateRelation(relation);
            _relations[relationId] = new RelationWrapper(relation, null);
            foreach (Role r in roles)
            {
                UpdateRoleMap(relationId, r, null);
            }
        }

        public void CreateRelationType(string relationTypeName, IEnumerable<RoleInfo> roleInfos)
        {
            AssertNotNull(relationTypeName, "relationTypeName");
            AssertNotNull(roleInfos, "roleInfos");
            RelationTypeSupport type = new RelationTypeSupport(relationTypeName, roleInfos);
            ValidateRelationType(type);
            _relationTypes[relationTypeName] = type;
        }

        public IDictionary<ObjectName, IList<string>> FindAssociatedMBeans(ObjectName objectName, string relationTypeName, string roleName)
        {
            AssertNotNull(objectName, "objectName");
            var results = new Dictionary<ObjectName, IList<string>>();
            foreach (RelationWrapper wrapper in _relations.Values)
            {
                var rel = wrapper.Relation;
                if (relationTypeName == null || rel.RelationTypeName == relationTypeName)
                {
                    IList<string> roles;
                    IDictionary<ObjectName, IList<string>> referenced = rel.ReferencedMBeans;
                    if (referenced.ContainsKey(objectName))
                    {
                        roles = referenced[objectName];
                        if (roleName == null || roles.Contains(roleName))
                        {
                            AddAssociatedMBeans(results, referenced.Keys, objectName, rel.Id);
                        }
                    }
                }
            }
            return results;
        }

        #region Utility
        private static void AddAssociatedMBeans(Dictionary<ObjectName, IList<string>> results, IEnumerable<ObjectName> toAdd, ObjectName exclude, string relationId)
        {
            foreach (ObjectName name in toAdd)
            {
                if (name != exclude)
                {
                    AddAssociatedMBean(results, name, relationId);
                }
            }
        }
        private static void AddAssociatedMBean(Dictionary<ObjectName, IList<string>> results, ObjectName name, string relationId)
        {
            IList<string> relationIds;
            if (results.TryGetValue(name, out relationIds))
            {
                relationIds.Add(relationId);
            }
            else
            {
                relationIds = new List<string>();
                relationIds.Add(relationId);
                results[name] = relationIds;
            }
        }
        #endregion

        public IDictionary<string, IList<string>> FindReferencingRelations(ObjectName objectName, string relationTypeName, string roleName)
        {
            AssertNotNull(objectName, "objectName");
            var results = new Dictionary<string, IList<string>>();
            foreach (RelationWrapper wrapper in _relations.Values)
            {
                var rel = wrapper.Relation;
                if (relationTypeName == null || rel.RelationTypeName == relationTypeName)
                {
                    IList<string> roles;
                    IDictionary<ObjectName, IList<string>> referenced = rel.ReferencedMBeans;
                    if (referenced.ContainsKey(objectName))
                    {
                        roles = referenced[objectName];
                        if (roleName == null)
                        {
                            AddRelationRoles(results, rel.Id, roles);
                        }
                        else if (roles.Contains(roleName))
                        {
                            AddRelationRole(results, rel.Id, roleName);
                        }
                    }
                }
            }
            return results;
        }

        #region Utility
        private static void AddRelationRoles(Dictionary<string, IList<string>> results, string relationId, IEnumerable<string> roleNames)
        {
            IList<string> relationRoles;
            if (results.TryGetValue(relationId, out relationRoles))
            {
                foreach (string roleName in roleNames)
                {
                    relationRoles.Add(roleName);
                }
            }
            else
            {
                relationRoles = new List<string>();
                foreach (string roleName in roleNames)
                {
                    relationRoles.Add(roleName);
                }
                results[relationId] = relationRoles;
            }
        }

        private static void AddRelationRole(Dictionary<string, IList<string>> results, string relationId, string roleName)
        {
            AddRelationRoles(results, relationId, new string[] { roleName });
        }
        #endregion

        public IList<string> FindRelationsOfType(string relationTypeName)
        {
            AssertNotNull(relationTypeName, "relationTypeName");

            var results = new List<string>();
            foreach (RelationWrapper wrapper in _relations.Values)
            {
                var rel = wrapper.Relation;
                if (rel.RelationTypeName == relationTypeName)
                {
                    results.Add(rel.Id);
                }
            }
            return results;
        }

        public IList<string> GetAllRelationIds()
        {
            return new List<string>(_relations.Keys);
        }

        public IList<string> GetAllRelationTypeNames()
        {
            return new List<string>(_relationTypes.Keys);
        }

        public RoleResult GetAllRoles(string relationId)
        {
            return GetRelation(relationId).Roles;
        }

        public bool Purge
        {
            get
            {
                return _purge;
            }
            set
            {
                _purge = value;
            }
        }

        public IDictionary<ObjectName, IList<string>> GetReferencedMBeans(string relationId)
        {
            return GetRelation(relationId).ReferencedMBeans;
        }

        public string GetRelationTypeName(string relationId)
        {
            return GetRelation(relationId).RelationTypeName;
        }

        public IList<ObjectName> GetRole(string relationId, string roleName)
        {
            return GetRelation(relationId)[roleName];
        }

        public int GetRoleCardinality(string relationId, string roleName)
        {
            return GetRelation(relationId).GetRoleCardinality(roleName);
        }

        public RoleInfo GetRoleInfo(string relationTypeName, string roleInfoName)
        {
            return GetRelationType(relationTypeName)[roleInfoName];
        }

        public IList<RoleInfo> GetRoleInfos(string relationTypeName)
        {
            return GetRelationType(relationTypeName).RoleInfos;
        }

        public RoleResult GetRoles(string relationId, IEnumerable<string> roleNames)
        {
            return GetRelation(relationId).GetRoles(roleNames);
        }

        public bool HasRelation(string relationId)
        {
            return _relations.ContainsKey(relationId);
        }

        public bool Active
        {
            get { return _server != null; }
        }

        public string IsRelation(ObjectName objectName)
        {
            AssertNotNull(objectName, "objectName");
            dynamic rel;
            return _relationBeans.TryGetValue(objectName, out rel) 
                ? rel.Id 
                : null;
        }

        public ObjectName IsRelationMBean(string relationId)
        {
            return GetRelationWrapper(relationId).Name;
        }

        public void PurgeRelations()
        {
            Dictionary<string, Dictionary<string, List<ObjectName>>> toRemove = new Dictionary<string, Dictionary<string, List<ObjectName>>>();
            foreach (ObjectName removedBean in _removedBeans)
            {
                Dictionary<string, IList<string>> referencingThisBean = _referencingRelations[removedBean];
                foreach (string relationName in referencingThisBean.Keys)
                {
                    IList<string> rolesInThisRelation = referencingThisBean[relationName];
                    foreach (string roleName in rolesInThisRelation)
                    {
                        Dictionary<string, List<ObjectName>> toRemoveInThisRelation;
                        if (!toRemove.TryGetValue(relationName, out toRemoveInThisRelation))
                        {
                            toRemoveInThisRelation = new Dictionary<string, List<ObjectName>>();
                            toRemove[relationName] = toRemoveInThisRelation;
                        }
                        List<ObjectName> toRemoveInThisRole;
                        if (!toRemoveInThisRelation.TryGetValue(roleName, out toRemoveInThisRole))
                        {
                            toRemoveInThisRole = new List<ObjectName>();
                            toRemoveInThisRelation[roleName] = toRemoveInThisRole;
                        }
                        toRemoveInThisRole.Add(removedBean);
                    }
                }
            }
            foreach (string relationName in toRemove.Keys)
            {
                Dictionary<string, List<ObjectName>> toRemoveInThisRelation = toRemove[relationName];
                foreach (string roleName in toRemoveInThisRelation.Keys)
                {
                    SetRole(relationName, new Role(roleName, toRemoveInThisRelation[roleName]));
                }
            }
        }

        //private bool ValidateOwnRelation(IRelation relation)
        //{
        //   RoleResult roles = relation.RetrieveAllRoles();
        //   IRelationType type = _relationTypes[relation.RelationTypeName];
        //   foreach (Role r in roles.Roles)
        //   {
        //      RoleInfo info = type[r.Name];
        //      if (!Role.ValidateRole(r.Value, type[r.Name], _server))
        //      {
        //         return false;
        //      }
        //   }
        //}

        public void RemoveRelation(string relationId)
        {
            AssertRegistered();
            RelationWrapper wrapper = GetRelationWrapper(relationId);
            string typeName = wrapper.Relation.RelationTypeName;
            _relations.Remove(relationId);
            foreach (Role r in wrapper.Relation.RetrieveAllRoles().Roles)
            {
                UpdateRoleMap(relationId, null, r.Value);
            }
            RelationNotification notif = RelationNotification.CreateForRemoval(this, -1, relationId, typeName, wrapper.Name);
            this.SendNotification(notif);
        }

        public void RemoveRelationType(string relationTypeName)
        {
            AssertRegistered();
            IRelationType type = GetRelationType(relationTypeName);
            _relationTypes.Remove(relationTypeName);
        }

        //public void SendRelationCreationNotification(string relationId)
        //{
        //   throw new Exception("The method or operation is not implemented.");
        //}

        //public void SendRelationRemovalNotification(string relationId, IEnumerable<ObjectName> unregMBeans)
        //{
        //   throw new Exception("The method or operation is not implemented.");
        //}

        //public void SendRoleUpdateNotification(string relationId, Role newRole, IList<ObjectName> oldRoleValue)
        //{
        //   throw new Exception("The method or operation is not implemented.");
        //}

        public void SetRole(string relationId, Role role)
        {
            AssertRegistered();
            RelationWrapper wrapper = GetRelationWrapper(relationId);
            string typeName = wrapper.Relation.RelationTypeName;
            IList<ObjectName> oldValue = wrapper.Relation[role.Name];
            wrapper.Relation[role.Name] = role.Value;
            UpdateRoleMap(relationId, role, oldValue);
            RelationNotification notif = RelationNotification.CreateForUpdate(this, -1, relationId, typeName, role.Name, wrapper.Name, role.Value, oldValue);
            SendNotification(notif);
        }

        public RoleResult SetRoles(string relationId, IEnumerable<Role> roles)
        {
            AssertRegistered();
            RelationWrapper wrapper = GetRelationWrapper(relationId);
            string typeName = wrapper.Relation.RelationTypeName;
            Dictionary<string, IList<ObjectName>> oldValues = new Dictionary<string, IList<ObjectName>>();
            foreach (Role r in roles)
            {
                try
                {
                    oldValues[r.Name] = wrapper.Relation[r.Name];
                }
                catch (RoleNotFoundException)
                {
                    oldValues[r.Name] = null;
                }
            }
            RoleResult result = wrapper.Relation.SetRoles(roles);
            foreach (Role r in roles)
            {
                UpdateRoleMap(relationId, r, oldValues[r.Name]);
            }
            foreach (Role r in roles)
            {
                SendNotification(RelationNotification.CreateForUpdate(this, -1, relationId, typeName, r.Name, wrapper.Name, r.Value, oldValues[r.Name]));
            }
            return result;
        }

        //public void UpdateRoleMap(string relationId, Role newRole, IList<ObjectName> oldRoleValue)
        //{
        //   throw new Exception("The method or operation is not implemented.");
        //}
        #endregion

        #region IMBeanRegistration Members
        public void PostDeregister()
        {
        }
        public void PostRegister(bool registrationDone)
        {
            _server.AddNotificationListener(MBeanServerDelegate.ObjectName, _ownName, null, null);
        }
        public void PreDeregister()
        {
            _server.RemoveNotificationListener(MBeanServerDelegate.ObjectName, _ownName);
        }
        public ObjectName PreRegister(IMBeanServer server, ObjectName name)
        {
            _server = server;
            _ownName = name;
            MBeanNotificationInfo info = new MBeanNotificationInfo(new string[]
            {
               RelationNotification.RelationBasicCreation,
               RelationNotification.RelationBasicRemoval,
               RelationNotification.RelationBasicUpdate,
               RelationNotification.RelationMBeanCreation,
               RelationNotification.RelationMBeanRemoval,
               RelationNotification.RelationMBeanUpdate
            }, typeof(RelationNotification).AssemblyQualifiedName, "Relation notification");
            this.Initialize(name, new MBeanNotificationInfo[] { info });
            return name;
        }

        #endregion

        #region INotificationListener Members
        public void HandleNotification(Notification notification, object handback)
        {
            MBeanServerNotification serverNotification = notification as MBeanServerNotification;
            if (serverNotification != null && serverNotification.Type == MBeanServerNotification.UnregistrationNotification)
            {
                if (_referencingRelations.ContainsKey(serverNotification.ObjectName))
                {
                    _removedBeans.Add(serverNotification.ObjectName);
                }
                if (_purge)
                {
                    PurgeRelations();
                }
                dynamic rel;
                if (_relationBeans.TryGetValue(serverNotification.ObjectName, out rel))
                {
                    RemoveRelation(rel.Id);
                }                
            }
        }
        #endregion

        #region Utility
        private void UpdateRoleMap(string relationId, Role newRole, IList<ObjectName> oldRoleValue)
        {
            if (newRole != null)
            {
                foreach (ObjectName name in newRole.Value)
                {
                    if (oldRoleValue == null || !oldRoleValue.Contains(name))
                    {
                        Dictionary<string, IList<string>> referencingThisBean;
                        if (!_referencingRelations.TryGetValue(name, out referencingThisBean))
                        {
                            referencingThisBean = new Dictionary<string, IList<string>>();
                            _referencingRelations[name] = referencingThisBean;
                        }
                        IList<string> rolesInThisRelation;
                        if (!referencingThisBean.TryGetValue(relationId, out rolesInThisRelation))
                        {
                            rolesInThisRelation = new List<string>();
                            referencingThisBean[relationId] = rolesInThisRelation;
                        }
                        rolesInThisRelation.Add(newRole.Name);
                    }
                }
                if (oldRoleValue != null)
                {
                    foreach (ObjectName name in oldRoleValue)
                    {
                        if (!newRole.Value.Contains(name))
                        {
                            IList<string> rolesInThisRelation = _referencingRelations[name][relationId];
                            if (rolesInThisRelation.Count > 1)
                            {
                                _referencingRelations[name][relationId].Remove(newRole.Name);
                            }
                            else
                            {
                                _referencingRelations[name].Remove(relationId);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (ObjectName name in oldRoleValue)
                {
                    _referencingRelations[name].Remove(relationId);
                }
            }
        }
        private void AssertNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
        private void AssertRegistered()
        {
            if (_server == null)
            {
                throw new RelationServiceNotRegisteredException();
            }
        }
        private IRelationType GetRelationType(string relationTypeName)
        {
            AssertNotNull(relationTypeName, "relationTypeName");
            IRelationType type;
            if (_relationTypes.TryGetValue(relationTypeName, out type))
            {
                return type;
            }
            else
            {
                throw new RelationTypeNotFoundException(relationTypeName);
            }
        }
        private RelationWrapper GetRelationWrapper(string relationId)
        {
            AssertNotNull(relationId, "relationId");
            RelationWrapper rel;
            if (_relations.TryGetValue(relationId, out rel))
            {
                return rel;
            }
            throw new RelationNotFoundException(relationId);
        }
        private dynamic GetRelation(string relationId)
        {
            return GetRelationWrapper(relationId).Relation;
        }
        private void ValidateRelation(dynamic relation)
        {
            if (_relations.ContainsKey(relation.Id))
            {
                throw new InvalidRelationIdException(relation.Id);
            }
            if (relation.RelationServiceName != _ownName)
            {
                throw new InvalidRelationServiceException(relation.RelationServiceName);
            }
            string relationType = relation.RelationTypeName;
            if (relationType == null || !_relationTypes.ContainsKey(relationType))
            {
                throw new RelationTypeNotFoundException(relationType);
            }
            RoleResult roles = relation.RetrieveAllRoles();
            IRelationType type = _relationTypes[relationType];
            foreach (Role r in roles.Roles)
            {
                RoleInfo info = type[r.Name];
                if (!Role.ValidateRole(r.Value, type[r.Name], _server))
                {
                    throw new InvalidRoleValueException();
                }
            }
        }
        private void ValidateRelationType(IRelationType type)
        {
            if (_relationTypes.ContainsKey(type.Name))
            {
                throw new InvalidRelationTypeException(type.Name, "Relation type with that name already registered.");
            }
            if (type.RoleInfos.Count == 0)
            {
                throw new InvalidRelationTypeException(type.Name, "Relation type contains no roles.");
            }
            foreach (RoleInfo info in type.RoleInfos)
            {
                if (info == null)
                {
                    throw new InvalidRelationTypeException(type.Name, "Relation type contains null RoleInfo.");
                }
                foreach (RoleInfo i in type.RoleInfos)
                {
                    if (!object.ReferenceEquals(i, info) && i.Name == info.Name)
                    {
                        throw new InvalidRelationTypeException(type.Name, "Relation type contains two roles with same name.");
                    }
                }
            }
        }
        #endregion

        #region Utility classes
        private class RelationWrapper
        {
            private readonly dynamic _relation;
            public dynamic Relation
            {
                get { return _relation; }
            }
            private readonly ObjectName _name;
            public ObjectName Name
            {
                get { return _name; }
            }
            public RelationWrapper(dynamic relation, ObjectName name)
            {
                _relation = relation;
                _name = name;
            }
        }
        #endregion
    }
}
