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
         if (_values.TryGetValue(fieldName, out value))
         {
            return value;
         }
         return null;
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
   }
}