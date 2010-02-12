using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Special dictionary object designed to hold string -> object mapping.
   /// </summary>
   public class Descriptor
   {
      private readonly Dictionary<string, object> _values = new Dictionary<string, object>();      

      public object GetFieldValue(string fieldName)
      {
         object value;
         return _values.TryGetValue(fieldName, out value) ? value : null;
      }

      public bool HasValue(string fieldName)
      {
         return _values.ContainsKey(fieldName);
      }
      
      public IEnumerable<string> GetFieldNames()
      {
         return _values.Keys;
      }

      public bool HasValue<T>(DescriptorField<T> field)
      {
         return HasValue(field.Name);
      }

      public T GetFieldValue<T>(DescriptorField<T> field)
      {
         return (T) GetFieldValue(field.Name);
      }

      public void SetField(string fieldName, object value)
      {
         _values[fieldName] = value;
      }

      public void SetField<T>(DescriptorField<T> field, object value)
      {
         _values[field.Name] = value;
      }

      public override bool Equals(object obj)
      {
         Descriptor other = obj as Descriptor;
         return other != null &&                
               _values.Count == other._values.Count &&
               _values.Keys.All(x => other._values.Keys.Contains(x) && _values[x].Equals(other._values[x]));
      }

      public override int GetHashCode()
      {
         return _values.Aggregate(0, (hash, value) => hash ^ value.Key.GetHashCode() ^ value.Value.GetHashCode());                
      }
   }
}