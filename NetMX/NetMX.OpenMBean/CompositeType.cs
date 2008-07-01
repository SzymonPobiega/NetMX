using System;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   [Serializable]
   public class CompositeType : OpenType
   {
      #region Members
      private readonly Dictionary<string, CompositeTypeMember> _members = new Dictionary<string, CompositeTypeMember>();
      #endregion

      #region Constructor
      /// <summary>
      /// Constructs a CompositeType instance, checking for the validity of the given parameters. 
      /// The validity constraints are described below for each parameter.
      /// Note that the contents of the three array parameters <paramref name="itemNames"/>, 
      /// <paramref name="itemDescriptions"/> and <paramref name="itemTypes"/>  are internally copied so that 
      /// any subsequent modification of these arrays by the caller of this constructor has no impact on the 
      /// constructed CompositeType instance.
      /// </summary>
      /// <param name="typeName">The name given to the composite type this instance represents; cannot be a 
      /// null or empty string.</param>
      /// <param name="description">The human readable description of the composite type this instance 
      /// represents; cannot be a null or empty string. </param>
      /// <param name="itemNames">The names of the items contained in the composite data values described by 
      /// this CompositeType instance; cannot be null and should contain at least one element; no element can 
      /// be a null or empty string. Note that the order in which the item names are given is not important 
      /// to differentiate a CompositeType instance from another; the item names are internally stored sorted 
      /// in ascending alphanumeric order. </param>
      /// <param name="itemDescriptions">The descriptions, in the same order as <paramref name="itemNames"/>, 
      /// of the items contained in the composite data values described by this CompositeType instance; should 
      /// be of the same size as itemNames; no element can be a null or empty string. </param>
      /// <param name="itemTypes">The open type instances, in the same order as itemNames, describing the items 
      /// contained in the composite data values described by this CompositeType instance; should be of the 
      /// same size as <paramref name="itemNames"/>; no element can be null.</param>
      public CompositeType(string typeName, string description, IEnumerable<string> itemNames,
         IEnumerable<string> itemDescriptions, IEnumerable<OpenType> itemTypes)
         : base(typeof(ICompositeData), typeName, description)
      {
         if (itemNames == null)
         {
            throw new ArgumentNullException("itemNames");
         }
         if (itemDescriptions == null)
         {
            throw new ArgumentNullException("itemDescriptions");
         }
         if (itemTypes == null)
         {
            throw new ArgumentNullException("itemTypes");
         }
         IEnumerator<string> descriptions = itemDescriptions.GetEnumerator();
         IEnumerator<OpenType> types = itemTypes.GetEnumerator();
         foreach (string name in itemNames)
         {
            descriptions.MoveNext();
            types.MoveNext();
            if (string.IsNullOrEmpty(name))
            {
               throw new ArgumentNullException("itemNames", "Item names cannot contain null or empty string items.");
            }
            if (string.IsNullOrEmpty(descriptions.Current))
            {
               throw new ArgumentNullException("itemDescriptions", "Item descriptions cannot contain null or empty string items.");
            }
            if (types.Current == null)
            {
               throw new ArgumentNullException("itemTypes", "Item types cannot contain null items.");
            }
            if (_members.ContainsKey(name))
            {
               throw new OpenDataException("CompositeType items cannot have duplicate keys.");
            }
            _members[name] = new CompositeTypeMember(descriptions.Current, types.Current);
         }
      }
      #endregion

      #region Interface
      /// <summary>
      /// Returns the open type of the item whose name is <paramref name="itemName"/>, or null if this CompositeType instance 
      /// does not define any item whose name is <paramref name="itemName"/>.
      /// </summary>
      /// <param name="itemName">The name of the time.</param>
      /// <returns>The type.</returns>
      public OpenType GetOpenType(string itemName)
      {
         if (string.IsNullOrEmpty(itemName))
         {
            throw new ArgumentNullException("itemName");
         }
         CompositeTypeMember member;
         if (_members.TryGetValue(itemName, out member))
         {
            return member.Type;
         }
         else
         {
            return null;
         }
      }
      /// <summary>
      /// Returns the description of the item whose name is <paramref name="itemName"/>, or null if this CompositeType instance 
      /// does not define any item whose name is <paramref name="itemName"/>.
      /// </summary>
      /// <param name="itemName">The name of the item.</param>
      /// <returns>The description</returns>
      public string GetDescription(string itemName)
      {
         if (string.IsNullOrEmpty(itemName))
         {
            throw new ArgumentNullException("itemName");
         }
         CompositeTypeMember member;
         if (_members.TryGetValue(itemName, out member))
         {
            return member.Description;
         }
         else
         {
            return null;
         }
      }
      /// <summary>
      /// Returns true if this CompositeType instance defines an item whose name is <paramref name="itemName"/>.
      /// </summary>
      /// <param name="itemName">The name of the item.</param>
      /// <returns>True if an item of this name is present.</returns>
      public bool ContainsKey(string itemName)
      {
         if (string.IsNullOrEmpty(itemName))
         {
            throw new ArgumentNullException("itemName");
         }
         return _members.ContainsKey(itemName);
      }
      /// <summary>
      /// Returns an unmodifiable view of all the item names defined by this CompositeType instance.       
      /// </summary>
      public ICollection<string> KeySet
      {
         get
         {
            return _members.Keys;
         }
      }
      #endregion

      #region Overridden
      public sealed override void Visit(OpenTypeVisitor visitor)
      {
         visitor.VisitCompositeType(this);
      }
      public sealed override bool IsValue(object value)
      {
         ICompositeData composite = value as ICompositeData;
         return composite != null && composite.CompositeType.Equals(this);
      }
      public sealed override OpenTypeKind Kind
      {
         get { return OpenTypeKind.CompositeType; }
      }
      public sealed override bool Equals(object obj)
      {
         CompositeType other = obj as CompositeType;
         if (other != null && TypeName.Equals(other.TypeName) && other._members.Count == _members.Count)
         {
            foreach (string key in _members.Keys)
            {
               CompositeTypeMember thisMember = _members[key];
               CompositeTypeMember otherMember;
               if (!other._members.TryGetValue(key, out otherMember))
               {
                  return false;
               }
               if (!thisMember.Equals(otherMember))
               {
                  return false;
               }
            }
            return true;
         }
         else
         {
            return false;
         }
      }
      public sealed override int GetHashCode()
      {
         int code = TypeName.GetHashCode();
         foreach (string key in _members.Keys)
         {
            code ^= key.GetHashCode();
            code ^= _members[key].GetHashCode();
         }
         return code;
      }
      #endregion

      #region Nested class
      [Serializable]
      private class CompositeTypeMember
      {
         private readonly string _description;
         public string Description
         {
            get { return _description; }
         }
         private readonly OpenType _type;
         public OpenType Type
         {
            get { return _type; }
         }         
         public CompositeTypeMember(string description, OpenType type)
         {
            _description = description;
            _type = type;
         }
         public override bool Equals(object obj)
         {
            CompositeTypeMember other = obj as CompositeTypeMember;
            return other != null && _type.Equals(other._type);
         }
         public override int GetHashCode()
         {
            return _type.GetHashCode();
         }
      }
      #endregion
   }
}
