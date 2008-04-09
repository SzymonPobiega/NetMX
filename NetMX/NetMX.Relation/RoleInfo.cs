using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Relation
{
   /// <summary>
   /// A RoleInfo object summarises a role in a relation type.
   /// </summary>
   public sealed class RoleInfo
   {
      #region Const
      public const int RoleCardinalityInfinity = int.MaxValue;
      #endregion

      #region Properties
      private int _minDegree;
      /// <summary>
      /// Gets minimum degree for corresponding role reference.
      /// </summary>
      public int MinDegree
      {
         get { return _minDegree; }
      }
      private int _maxDegree;
      /// <summary>
      /// Gets maximum degree for corresponding role reference.
      /// </summary>
      public int MaxDegree
      {
         get { return _maxDegree; }
      }
      private string _name;
      /// <summary>
      /// Gets the name of the role.
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private string _description;
      /// <summary>
      /// Gets description text for the role.
      /// </summary>
      public string Description
      {
         get { return _description; }
      }
      private bool _readable;
      /// <summary>
      /// Gets read access mode for the role (true if it is readable).
      /// </summary>
      public bool Readable
      {
         get { return _readable; }
      }
      private bool _writable;
      /// <summary>
      /// Gets write access mode for the role (true if it is writable).
      /// </summary>
      public bool Writable
      {
         get { return _writable; }
      }
      private string _refMBeanClassName;
      /// <summary>
      /// Gets name of type of MBean expected to be referenced in corresponding role.
      /// </summary>
      public string RefMBeanClassName
      {
         get { return _refMBeanClassName; }
         set { _refMBeanClassName = value; }
      }
      #endregion

      #region Constructors
      public RoleInfo(string name, Type refMBeanClass)
         : this(name, refMBeanClass, true, true, 1, 1, null)
      {
      }
      public RoleInfo(string name, string refMBeanClassName)
         : this(name, refMBeanClassName, true, true, 1, 1, null)
      {
      }
      public RoleInfo(string name, Type refMBeanClass, bool readable, bool writable)
         : this(name, refMBeanClass, readable, writable, 1, 1, null)
      {
      }
      public RoleInfo(string name, string refMBeanClassName, bool readable, bool writable)
         : this(name, refMBeanClassName, readable, writable, 1, 1, null)
      {
      }
      public RoleInfo(string name, Type refMBeanClass, bool readable, bool writable, 
         int minDegree, int maxDegree, string description)
         : this(name, refMBeanClass.AssemblyQualifiedName, readable, writable, minDegree, maxDegree, description)
      {
      }
      /// <summary>
      /// Creates new RoleInfo object.
      /// </summary>
      /// <param name="name">The name of the role.</param>
      /// <param name="refMBeanClassName">Name of type of MBean expected to be referenced in corresponding role.</param>
      /// <param name="readable">Read access mode for the role (true if it is readable).</param>
      /// <param name="writable">Write access mode for the role (true if it is writable).</param>
      /// <param name="minDegree">Minimum degree for corresponding role reference.</param>
      /// <param name="maxDegree">Maximum degree for corresponding role reference.</param>
      /// <param name="description">Description text for the role.</param>
      /// <exception cref="NetMX.Relation.InvalidRoleInfoException">If the minimum degree is greater than the maximum degree.</exception>      
      public RoleInfo(string name, string refMBeanClassName, bool readable, bool writable, int minDegree, int maxDegree, string description)
      {
         if (name == null)
         {
            throw new ArgumentNullException("name");
         }
         if (refMBeanClassName == null)
         {
            throw new ArgumentNullException("refMBeanClassName");
         }
         if (minDegree < 0)
         {
            throw new ArgumentOutOfRangeException("minDegree", minDegree, "Degree value must not be negative.");
         }
         if (maxDegree < 0)
         {
            throw new ArgumentOutOfRangeException("maxDegree", maxDegree, "Degree value must not be negative.");
         }
         if (maxDegree < minDegree)
         {
            throw new InvalidRoleInfoException(name, minDegree, maxDegree);
         }
         _name = name;
         _refMBeanClassName = refMBeanClassName;
         _readable = readable;
         _writable = writable;
         _minDegree = minDegree;
         _maxDegree = maxDegree;
         _description = description;
      }
      #endregion

      #region Interface
      public bool CheckMaxDegree(int value)
      {
         if (value < 0)
         {
            throw new ArgumentOutOfRangeException("value", value, "Degree value must not be negative.");
         }
         return value <= _maxDegree;         
      }
      public bool CheckMinDegree(int value)
      {
         if (value < 0)
         {
            throw new ArgumentOutOfRangeException("value", value, "Degree value must not be negative.");
         }
         return value >= _minDegree;
      }
      #endregion
   }
}
