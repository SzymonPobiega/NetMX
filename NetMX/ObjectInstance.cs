#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Used to represent the object name of an MBean and its class name. If the MBean is a Dynamic 
   /// MBean the class name should be retrieved from the <see cref="MBeanInfo"/> it provides.
   /// </summary>
   [Serializable]   
   public sealed class ObjectInstance
   {
      #region PROPERTIES
      private string _className;
      /// <summary>
      /// Gets the class name of MBean.
      /// </summary>
      public string ClassName
      {
         get { return _className; }
      }
      private ObjectName _objectName;
      /// <summary>
      /// Gets the object name of MBean.
      /// </summary>
      public ObjectName ObjectName
      {
         get { return _objectName; }
      }
      #endregion      

      #region CONSTRUCTOR
      /// <summary>
      /// Allows an object instance to be created given an object name and the full class name, 
      /// including the assembly name.
      /// </summary>
      /// <param name="objectName">Object name.</param>
      /// <param name="className">Assembly qualified class name.</param>
      public ObjectInstance(ObjectName objectName, string className)
      {
         _objectName = objectName;
         _className = className;
      }
      #endregion

      #region Overridden
      /// <summary>
      /// Compares ObjectName instances using ClassName and ObjectName properties.
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         ObjectInstance other = obj as ObjectInstance;
         if (other != null &&
            this._className.Equals(other._className) &&
            this._objectName.Equals(other._objectName))
         {
            return true;
         }
         return false;
      }
      /// <summary>
      /// Uses ObjectName and ClassName properties to calculate hash code.
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return _objectName.GetHashCode() ^ _className.GetHashCode();
      }
      #endregion

      #region Operators
      public static bool operator ==(ObjectInstance left, ObjectInstance right)
      {
         return (Object.ReferenceEquals(left, null) && Object.ReferenceEquals(right, null)) ||
            (!Object.ReferenceEquals(left, null) && !Object.ReferenceEquals(right, null) && left.Equals(right));         
      }
      public static bool operator !=(ObjectInstance left, ObjectInstance right)
      {
         return !(left == right);
      }
      #endregion
   }
}
