#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Relation
{
   [Serializable]
   public class RelationNotification : Notification
   {
      #region Const
      public const string RelationBasicCreation = "netmx.relation.creation.basic";
      public const string RelationBasicRemoval = "netmx.relation.removal.basic";
      public const string RelationBasicUpdate = "netmx.relation.update.basic";
      public const string RelationMBeanCreation = "netmx.relation.creation.mbean";
      public const string RelationMBeanRemoval = "netmx.relation.removal.mbean";
      public const string RelationMBeanUpdate = "netmx.relation.update.mbean";
      #endregion

      #region MEMBERS
      #endregion

      #region PROPERTIES
      private ObjectName _objectName;
      /// <summary>
      /// Gets the ObjectName of the created/removed/updated relation (if relation is implemented as an MBean).
      /// </summary>
      public ObjectName ObjectName
      {
         get { return _objectName; }
      }
      private IList<ObjectName> _newRoleValue;
      /// <summary>
      /// Gets new value of updated role (only for role update).
      /// </summary>
      public IList<ObjectName> NewRoleValue
      {
         get { return _newRoleValue; }
      }
      private IList<ObjectName> _oldRoleValue;
      /// <summary>
      /// Gets old value of updated role (only for role update).
      /// </summary>
      public IList<ObjectName> OldRoleValue
      {
         get { return _oldRoleValue; }
      }
      private string _relationId;
      /// <summary>
      /// Gets the relation identifier of created/removed/updated relation.
      /// </summary>
      public string RelationId
      {
         get { return _relationId; }
      }
      private string _relationTypeName;
      /// <summary>
      /// Gets the relation type name of created/removed/updated relation.
      /// </summary>
      public string RelationTypeName
      {
         get { return _relationTypeName; }
      }
      private string _roleName;
      /// <summary>
      /// Gets name of updated role of updated relation (only for role update).
      /// </summary>
      public string RoleName
      {
         get { return _roleName; }
      }
      #endregion

      #region CONSTRUCTOR
      private RelationNotification(string type, object source, long sequenceNumber, string message,
         string relationId, string relationTypeName, string roleName,
         ObjectName objectName, IEnumerable<ObjectName> newRoleValue, IEnumerable<ObjectName> oldRoleValue)
         : base(type, source, sequenceNumber, message, null)
      {
         _relationId = relationId;
         _relationTypeName = relationTypeName;
         _roleName = roleName;
         _objectName = objectName;
         if (newRoleValue != null)
         {
            _newRoleValue = new List<ObjectName>(newRoleValue).AsReadOnly();
         }
         if (oldRoleValue != null)
         {
            _oldRoleValue = new List<ObjectName>(oldRoleValue).AsReadOnly();
         }
      }
      public static RelationNotification CreateForCreation(object source, long sequenceNumber,
         string relationId, string relationTypeName, ObjectName objectName)
      {
         return new RelationNotification(objectName == null ? RelationBasicCreation : RelationMBeanCreation,
            source, sequenceNumber, "Relation created.", relationId, relationTypeName, null, objectName, null, null);
      }
      public static RelationNotification CreateForRemoval(object source, long sequenceNumber,
               string relationId, string relationTypeName, ObjectName objectName)
      {
         return new RelationNotification(objectName == null ? RelationBasicRemoval : RelationMBeanRemoval,
            source, sequenceNumber, "Relation removed.", relationId, relationTypeName, null, objectName, null, null);
      }
      public static RelationNotification CreateForUpdate(object source, long sequenceNumber,
               string relationId, string relationTypeName, string roleName, ObjectName objectName, IEnumerable<ObjectName> newRoleValue, IEnumerable<ObjectName> oldRoleValue)
      {
         return new RelationNotification(objectName == null ? RelationBasicUpdate : RelationMBeanUpdate,
            source, sequenceNumber, "Relation updated.", relationId, relationTypeName, roleName, objectName, newRoleValue, oldRoleValue);
      }
      #endregion
   }
}
