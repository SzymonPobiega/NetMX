#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
#endregion

namespace NetMX.Relation
{
   /// <summary>
   /// Represents a role: includes a role name and referenced MBeans (via their ObjectNames).       
   /// </summary>   
   /// <remarks>
   /// This class is immutable.
   /// </remarks>
   [Serializable]
   public sealed class Role //: ISerializable
   {      
      #region PROPERTIES
      private string _name;
      /// <summary>
      /// Gets or sets the name of the role.
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private ReadOnlyCollection<ObjectName> _value;
      /// <summary>
      /// Gets or sets the value of the role (referenced MBeans).
      /// </summary>
      public IList<ObjectName> Value
      {
         get { return _value; }         
      }
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Creates a new Role object. No check is made that the ObjectNames in the role value exist in an MBean 
      /// server. That check will be made when the role is set in a relation.      
      /// </summary>
      /// <param name="name">Name of the role.</param>
      /// <param name="value">Value of the role (referenced MBeans)</param>
      public Role(string name, IEnumerable<ObjectName> value)
      {
         _name = name;
         _value = new List<ObjectName>(value).AsReadOnly();
      }
      //private Role(SerializationInfo info, StreamingContext ctx)
      //{
      //   info.AddValue("name", _name);
      //   info.AddValue(_value.
      //}
      #endregion

      //#region ISerializable Members
      //public void GetObjectData(SerializationInfo info, StreamingContext context)
      //{
      //   _name = info.GetString("name");
      //   info.get
      //}
      //#endregion
   }
}
