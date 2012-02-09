#region USING
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The CompositeDataSupport class is the open data class which implements the <see cref="ICompositeData"/> interface.
   /// </summary>
   [Serializable]
   public class CompositeDataSupport : ICompositeData
   {
      #region MEMBERS
      private readonly Dictionary<string, object> _items = new Dictionary<string, object>();
      private readonly CompositeType _compositeType;
      #endregion

      #region PROPERTIES
      #endregion

      #region CONSTRUCTOR
      /// <summary>
      /// Constructs a CompositeDataSupport instance with the specified <paramref name="compositeType"/>, 
      /// whose item names and corresponding values are given by the mappings in the map <paramref name="items"/>.       
      /// </summary>
      /// <param name="compositeType">The composite type  of this composite data instance; must not be null.</param>
      /// <param name="items">The mappings of all the item names to their values; items must contain all the item 
      /// names defined in <paramref name="compositeType"/>; must not be null or empty.</param>
      public CompositeDataSupport(CompositeType compositeType, IDictionary<string, object > items)
         : this(compositeType,items != null ? items.Keys : null, items != null ? items.Values : null)
      {                  
      }
      /// <summary>
      /// Constructs a CompositeDataSupport instance with the specified <paramref name="compositeType"/>, 
      /// whose item values are specified by <paramref name="itemValues"/>, in the same order as in 
      /// <paramref name="itemNames"/>. As a <see cref="compositeType"/> does not specify any order on its 
      /// items, the <see cref="itemNames"/> parameter is used to specify the order in which the values are 
      /// given in <paramref name="itemValues"/>. 
      /// </summary>
      /// <param name="compositeType">The composite type  of this composite data instance; must not be null.</param>
      /// <param name="itemNames">Must list, in any order, all the item names defined in 
      /// <paramref name="compositeType"/>; the order in which the names are listed, is used to match values in 
      /// <paramref name="itemValues"/>; must not be null or empty.</param>
      /// <param name="itemValues">The values of the items, listed in the same order as their respective names 
      /// in <paramref name="itemNames"/>; each item value can be null, but if it is non-null it must be a 
      /// valid value for the open type defined in <paramref name="compositeType"/> for the corresponding item; 
      /// must be of the same size as <paramref name="itemNames"/>; must not be null or empty.</param>
      public CompositeDataSupport(CompositeType compositeType, IEnumerable<string> itemNames, IEnumerable<object > itemValues)
      {
         if (compositeType == null)
         {
            throw new ArgumentNullException("compositeType");
         }
         if (itemNames == null)
         {
            throw new ArgumentNullException("itemNames");
         }
         if (itemValues == null)
         {
            throw new ArgumentNullException("itemValues");
         }         
         IEnumerator<object> values = itemValues.GetEnumerator();
         foreach (string itemName in itemNames)
         {
            if (!values.MoveNext())
            {
               throw new OpenDataException("Names and value collections must have equal size.");
            }
            OpenType itemType = compositeType.GetOpenType(itemName);
            if (itemType == null)
            {
               throw new OpenDataException("Composite type doesn't have item with name "+itemName);
            }
            if (values.Current != null && !itemType.IsValue(values.Current))
            {
               throw new OpenDataException("Value is not valid for its item's open type.");
            }
            _items[itemName] = values.Current;
         }         
         if (_items.Count != compositeType.KeySet.Count)
         {
            throw new OpenDataException(string.Format(CultureInfo.CurrentCulture,
                                                      "Composite type has different item count ({0}) than count of items provided ({1}).",
                                                      _items.Count, compositeType.KeySet.Count));
         }
         _compositeType = compositeType;         
      }
      #endregion

      #region ICompositeData Members
      public bool ContainsKey(string key)
      {
         return _items.ContainsKey(key);
      }
      public bool ContainsValue(object value)
      {
         return _items.ContainsValue(value);
      }
      public object this[string key]
      {
         get
         {
            object result;
            if (_items.TryGetValue(key, out result))
            {
               return result;
            }
            throw new InvalidKeyException(key);
         }
      }
      public IList<object> GetAll(IEnumerable<string> keys)
      {
         List<object> results = new List<object>();
         foreach (string key in keys)
         {
            object item;
            if (_items.TryGetValue(key, out item))
            {
               results.Add(item);
            }
            else
            {
               throw new InvalidKeyException(key);
            }
         }
         return results;
      }
      public CompositeType CompositeType
      {
         get { return _compositeType; }
      }
      public IEnumerable<object> Values
      {
         get { return _items.Keys.OrderBy(x => x).Select(x => _items[x]); }
      }
      #endregion

      public override bool Equals(object obj)
      {
         ICompositeData other = obj as ICompositeData;
         return other != null &&
                CompositeType.Equals(other.CompositeType) &&
                GetAll(CompositeType.KeySet.OrderBy(x => x)).SequenceEqual(other.GetAll(CompositeType.KeySet.OrderBy(x => x)));
      }

      public override int GetHashCode()
      {
         return _items.Aggregate(CompositeType.GetHashCode(),
                                 (acc, x) => acc ^ GetValuePairHashCode(x));
      }

      private static int GetValuePairHashCode(KeyValuePair<string, object> pair)
      {
         int code = pair.Key.GetHashCode();
         if (pair.Value != null)
         {
            code ^= pair.Value.GetHashCode();
         }
         return code;
      }
   }
}
