#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// A RelationTypeSupport object implements the RelationType interface.
   /// It represents a relation type, providing role information for each role expected to be supported in every relation of that type.
   /// A relation type includes a relation type name and a list of role infos (represented by RoleInfo objects).
   /// A relation type has to be declared in the Relation Service:
   /// <list type="bullet">
   /// <item>either using the <see cref="NetMX.Relation.RelationServiceMBean.CreateRelationType"/>() method, 
   /// where a RelationTypeSupport object will be created and kept in the Relation Service</item>
   /// <item>either using the <see cref="NetMX.Relation.RelationServiceMBean.AddRelationType"/>() method where 
   /// the user has to create an object implementing the RelationType interface, and this object will be used 
   /// as representing a relation type in the Relation Service.</item>
   /// </list>
   /// </summary>
   [Serializable]
   public sealed class RelationTypeSupport : IRelationType
   {
      #region MEMBERS
      private string _name;
      private ReadOnlyCollection<RoleInfo> _roleInfos;
      #endregion

      #region CONSTRUCTOR
      public RelationTypeSupport(string roleName, IEnumerable<RoleInfo> roleInfos)
      {
         _name = roleName;
         _roleInfos = new List<RoleInfo>(roleInfos).AsReadOnly();
      }
      #endregion

      #region IRelationType Members
      public string Name
      {
         get { return _name; }
      }
      public RoleInfo this[string roleName]
      {
         get 
         {
            if (roleName == null)
            {
               throw new ArgumentNullException("roleName");
            }
            foreach (RoleInfo info in _roleInfos)
            {
               if (info.Name == roleName)
               {
                  return info;
               }
            }
            throw new RoleInfoNotFoundException(roleName);
         }
      }
      public IList<RoleInfo> RoleInfos
      {
         get { return _roleInfos; }
      }
      #endregion
   }
}
