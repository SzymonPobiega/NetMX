#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.OpenMBean
{
   [Serializable]
   public class TabularDataSupport : ITabularData
   {
      #region MEMBERS
      private readonly TabularType _tabularType;
      private readonly Dictionary<IEnumerable<object >, ICompositeData> _rows;
      #endregion
      
      #region CONSTRUCTOR
      /// <summary>
      /// Creates new TabularDataSupport object.
      /// </summary>
      /// <param name="tabularType">The tabular type describing this ITabularData instance; cannot be null.</param>
      public TabularDataSupport(TabularType tabularType)
      {
         if (tabularType == null)
         {
            throw new ArgumentNullException("tabularType");
         }
         _rows = new Dictionary<IEnumerable<object>, ICompositeData>();
         _tabularType = tabularType;
      }
      /// <summary>
      /// Creates new TabularDataSupport object.
      /// </summary>
      /// <param name="tabularType">The tabular type describing this TabularData instance; cannot be null.</param>
      /// <param name="initialCapacity">The initial capacity of the underlying dictionary.</param>
      public TabularDataSupport(TabularType tabularType, int initialCapacity)
      {
         _rows = new Dictionary<IEnumerable<object>, ICompositeData>(initialCapacity);
         _tabularType = tabularType;
      }
      #endregion

      #region ITabularData Members
      public IList<object> CalculateIndex(ICompositeData value)
      {
         if (value == null)
         {
            throw new ArgumentNullException("value");
         }
         if (!_tabularType.RowType.IsValue(value))
         {
            throw new InvalidOpenTypeException(_tabularType.RowType, value);
         }
         List<object> results = new List<object>();
         foreach (string indexItem in _tabularType.IndexNames)
         {
            results.Add(value[indexItem]);
         }
         return results;
      }
      public void Clear()
      {
         _rows.Clear();
      }
      public bool ContainsKey(IEnumerable<object> key)
      {
         if (key == null)
         {
            throw new ArgumentNullException("key");
         }
         IndexEntry entry = new IndexEntry(key);
         return _rows.ContainsKey(entry);
      }
      public bool ContainsValue(ICompositeData value)
      {
         if (value == null)
         {
            throw new ArgumentNullException("value");
         }
         return _rows.ContainsValue(value);
      }
      public ICompositeData this[IEnumerable<object> key]
      {
         get
         {
            if (key == null)
            {
               throw new ArgumentNullException("key");
            }
            ICompositeData result;
            if (_rows.TryGetValue(new IndexEntry(key), out result ))
            {
               return result;
            }
            else
            {
               return null;
            }
         }
      }
      public TabularType TabularType
      {
         get { return _tabularType; }
      }
      public bool Empty
      {
         get { return _rows.Count == 0; }
      }
      public IEnumerable<IEnumerable<object>> Keys
      {
         get { return _rows.Keys;}
      }
      public void Put(ICompositeData value)
      {                  
         IndexEntry entry = new IndexEntry(CalculateIndex(value));
         if (_rows.ContainsKey(entry))
         {
            throw new KeyAlreadyExistsException(entry.Items);
         }
         _rows.Add(entry, value);
      }
      public void PutAll(IEnumerable<ICompositeData> values)
      {
         if (values == null)
         {
            throw new ArgumentException("values");
         }
         Dictionary<IndexEntry,ICompositeData > itemsToAdd = new Dictionary<IndexEntry, ICompositeData>();
         foreach (ICompositeData value in values)
         {
            IndexEntry entry = new IndexEntry(CalculateIndex(value));
            if (_rows.ContainsKey(entry) || itemsToAdd.ContainsKey(entry))
            {
               throw new KeyAlreadyExistsException(entry.Items);
            }
            itemsToAdd.Add(entry, value);
         }
         foreach (KeyValuePair<IndexEntry, ICompositeData> pair in itemsToAdd)
         {
            _rows.Add(pair.Key, pair.Value);  
         }
      }
      public ICompositeData Remove(IEnumerable<object> key)
      {
         if (key == null)
         {
            throw  new ArgumentException("key");
         }
         ICompositeData result;
         IndexEntry entry = new IndexEntry(key);
         if (_rows.TryGetValue(entry, out result))
         {
            _rows.Remove(entry);
            return result;
         }
         else
         {
            return null;
         }
      }
      public int Count
      {
         get { return _rows.Count; }
      }
      public IEnumerable<ICompositeData> Values
      {
         get { return _rows.Values; }
      }
      #endregion

      #region Nested class
      [Serializable]
      private class IndexEntry : IEnumerable<object >
      {
         private readonly IEnumerable<object> _items;
         public IEnumerable<object> Items
         {
            get { return _items; }
         }
         public IndexEntry(IEnumerable<object> items)
         {
            _items = items;
         }
         public override bool Equals(object obj)
         {
            IndexEntry other = obj as IndexEntry;
            if (other != null)
            {
               IEnumerator<object> otherValues = other.Items.GetEnumerator();
               return Equals(otherValues);
            }
            else
            {                              
               return false;
            }
         }
         private bool Equals(IEnumerator<object> otherValues)
         {            
            foreach (object o in _items)
            {
               if (!otherValues.MoveNext())
               {
                  return false;
               }
               if (!((o == null && otherValues.Current == null) ||
                  (o != null && otherValues.Current != null && o.Equals(otherValues.Current))))
               {
                  return false;
               }
            }
            return !otherValues.MoveNext();
         }
         public override int GetHashCode()
         {
            int code = 0;
            foreach (object o in _items)
            {
               if (o != null)
               {
                  code ^= o.GetHashCode();
               }
            }
            return code;
         }

         #region IEnumerable<object> Members
         public IEnumerator<object> GetEnumerator()
         {
            return _items.GetEnumerator();
         }
         #endregion

         #region IEnumerable Members
         System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
         {
            return _items.GetEnumerator();
         }
         #endregion
      }
      #endregion
   }
}
