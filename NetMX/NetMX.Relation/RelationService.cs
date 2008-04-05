#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Relation
{
   public class RelationService : NotificationEmitterSupport, RelationServiceMBean, IMBeanRegistration
   {
      #region MEMBERS
      #endregion

      #region PROPERTIES
      #endregion

      #region CONSTRUCTOR
      #endregion

      #region RelationServiceMBean Members

      public void AddRelation(ObjectName relationObjectName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void AddRelationType(IRelationType relationType)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleStatus CheckRoleReading(string roleName, string relationTypeName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleStatus CheckRoleWriting(string roleName, string relationTypeName, bool initFlag)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void CreateRelation(string relationId, string roleTypeName, IEnumerable<Role> roles)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void CreateRelationType(string relationTypeName, IEnumerable<RoleInfo> roleInfos)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IDictionary<ObjectName, IList<string>> FindAssociatedMBeans(ObjectName objectName, string relationTypeName, string roleName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IDictionary<string, IList<string>> FindReferencingRelations(ObjectName objectName, string relationTypeName, string roleName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IList<string> FindRelationsOfType(string relationTypeName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IList<string> GetAllRelationIds()
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IList<string> GetAllRelationTypeNames()
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleResult GetAllRoles(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public bool Purge
      {
         get
         {
            throw new Exception("The method or operation is not implemented.");
         }
         set
         {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public IDictionary<ObjectName, IList<string>> GetReferencedMBeans(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public string GetRelationTypeName(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IList<ObjectName> GetRole(string relationId, string roleName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public int GetRoleCardinality(string relationId, string roleName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleInfo GetRoleInfo(string relationTypeName, string roleInfoName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public IList<RoleInfo> GetRoleInfos(string relationTypeName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleResult GetRoles(string relationId, IEnumerable<string> roleNames)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public bool HasRelation(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public bool IsActive
      {
         get { throw new Exception("The method or operation is not implemented."); }
      }

      public string IsRelation(ObjectName objectName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public ObjectName IsRelationMBean(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void PurgeRelations()
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void RemoveRelation(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void RemoveRelationType(string relationTypeName)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void SendRelationCreationNotification(string relationId)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void SendRelationRemovalNotification(string relationId, IEnumerable<ObjectName> unregMBeans)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void SendRoleUpdateNotification(string relationId, Role newRole, IList<ObjectName> oldRoleValue)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void SetRole(string relationId, Role role)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public RoleResult SetRoles(string relationId, IEnumerable<Role> roles)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void UpdateRoleMap(string relationId, Role newRole, IList<ObjectName> oldRoleValue)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      #endregion

      #region IMBeanRegistration Members

      public void PostDeregister()
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void PostRegister(bool registrationDone)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public void PreDeregister()
      {
         throw new Exception("The method or operation is not implemented.");
      }

      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         throw new Exception("The method or operation is not implemented.");
      }

      #endregion
   }
}
