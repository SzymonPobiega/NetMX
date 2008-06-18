using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Globalization;
using System.Collections;

namespace NetMX.OpenMBean.Mapper
{
   /// <summary>
   /// A mapper which maps CLR generic collection types (like IEnumerable<>) to their OpenMBean equivalents:
   /// <list type="bullet">
   /// <item>An array (<see cref="ArrayType"/>) if collection element maps to <see cref="SimpleType"/>,</item>
   /// <item>A table (<see cref="TabularType"/>) if it does not.</item>
   /// </list>
   /// </summary>
   public class CollectionTypeMapper : ITypeMapper
   {
      private const string CollectionIndexColumnName = "CollectionIndex";
      private static readonly Type[] _supportedCollectionTypes = new Type[]
      {
         typeof(IEnumerable<>),
         typeof(ICollection<>),
         typeof(IList<>),
         typeof(List<>),
         typeof(LinkedList<>),
         typeof(ReadOnlyCollection<>)
      };

      #region ITypeMapper Members
      public bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback)
      {
         mapsTo = OpenTypeKind.ArrayType;
         if (plainNetType.IsGenericType)
         {
            Type genericDef = plainNetType.GetGenericTypeDefinition();
            if (Array.Exists(_supportedCollectionTypes, delegate(Type t)
               {
                  return t == genericDef;
               }))
            {
               Type elementType = plainNetType.GetGenericArguments()[0];
               return CanHandleElementType(elementType, out mapsTo, canHandleNestedTypeCallback);
            }
         }
         else if (plainNetType.IsArray && plainNetType.GetArrayRank() == 1)
         {
            return CanHandleElementType(plainNetType.GetElementType(), out mapsTo, canHandleNestedTypeCallback);
         }
         return false;
      }      
      public OpenType MapType(Type plainNetType, MapTypeDelegate mapNestedTypeCallback)
      {
         Type elementType;
         if (plainNetType.IsArray)
         {
            elementType = plainNetType.GetElementType();            
         }
         else
         {
            elementType = plainNetType.GetGenericArguments()[0];
         }
         OpenTypeKind kind = ResolveMappedTypeKind(elementType);
         if (kind == OpenTypeKind.ArrayType)
         {
            return new ArrayType(1, mapNestedTypeCallback(elementType));
         }
         else
         {
            CompositeType mappedElementType = (CompositeType)mapNestedTypeCallback(elementType);
            CompositeType rowType = MakeRowType(mappedElementType);
            return new TabularType(
               string.Format(CultureInfo.InvariantCulture, "{0} table", AttributeUtils.GetOpenTypeName(elementType)),
               string.Format(CultureInfo.InvariantCulture, "Table of {0}", AttributeUtils.GetOpenTypeName(elementType)),
               rowType, new string[] { CollectionIndexColumnName });
         }
      }      
      public object MapValue(OpenType mappedType, object value, MapValueDelegate mapNestedValueCallback)
      {
         if (value == null)
         {
            return null;
         }
         if (mappedType.Kind == OpenTypeKind.ArrayType)
         {
            if (value.GetType().IsArray)
            {
               return value;
            }
            else
            {
               Type elementType = value.GetType().GetGenericArguments()[0];
               ArrayList result = new ArrayList();
               IEnumerable enumerableValue = (IEnumerable)value;
               foreach (object o in enumerableValue)
               {
                  result.Add(o);                  
               }
               return result.ToArray(elementType);
            }
         }
         else
         {
            TabularType tabularType = (TabularType) mappedType;
            ITabularData result = new TabularDataSupport(tabularType, 0);
            IEnumerable enumerableValue = (IEnumerable) value;
            int index = 0;
            foreach (object o in enumerableValue)
            {
               ICompositeData element = (ICompositeData) mapNestedValueCallback(MakeElementType(tabularType.RowType), o);               
               result.Put(MakeRowValue(element, index, tabularType.RowType));
               index++;
            }
            return result;            
         }
      }
      #endregion

      #region Utilities
      private static bool CanHandleElementType(Type elementType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback)
      {
         mapsTo = OpenTypeKind.ArrayType;
         OpenTypeKind elementMapsTo;
         if (canHandleNestedTypeCallback(elementType, out elementMapsTo))
         {
            mapsTo = ResolveMappedTypeKind(elementType);
            return (mapsTo == OpenTypeKind.ArrayType && elementMapsTo == OpenTypeKind.SimpleType) ||
                   (mapsTo == OpenTypeKind.TabularType && elementMapsTo == OpenTypeKind.CompositeType);
         }
         return false;
      }
      private static ICompositeData MakeRowValue(ICompositeData elementValue, int index, CompositeType rowType)
      {
         List<string> names = new List<string>();
         List<object> values = new List<object>();

         names.Add(CollectionIndexColumnName);
         values.Add(index);

         foreach (string itemName in elementValue.CompositeType.KeySet)
         {
            names.Add(itemName);
            values.Add(elementValue[itemName]);
         }

         return new CompositeDataSupport(rowType, names, values);
      }
      private static CompositeType MakeElementType(CompositeType rowType)
      {
         List<string> names = new List<string>();
         List<string> descriptions = new List<string>();
         List<OpenType> types = new List<OpenType>();

         foreach (string itemName in rowType.KeySet)
         {
            if (itemName != CollectionIndexColumnName)
            {
               names.Add(itemName);
               descriptions.Add(rowType.GetDescription(itemName));
               types.Add(rowType.GetOpenType(itemName));
            }
         }
         return new CompositeType(rowType.TypeName, rowType.Description, names, descriptions, types);
      }
      private static CompositeType MakeRowType(CompositeType elementType)
      {
         List<string> names = new List<string>();
         List<string> descriptions = new List<string>();
         List<OpenType> types = new List<OpenType>();

         names.Add(CollectionIndexColumnName);
         descriptions.Add("Index of a collection");
         types.Add(SimpleType.Integer);

         foreach (string itemName in elementType.KeySet)
         {
            names.Add(itemName);
            descriptions.Add(elementType.GetDescription(itemName));
            types.Add(elementType.GetOpenType(itemName));
         }
         return new CompositeType(elementType.TypeName, elementType.Description, names, descriptions, types);
      }
      private static OpenTypeKind ResolveMappedTypeKind(Type elementType)
      {
         if (elementType.IsPrimitive || elementType == typeof(string) || elementType == typeof(DateTime) || elementType == typeof(TimeSpan) || elementType == typeof(ObjectName))
         {
            return OpenTypeKind.ArrayType;
         }
         return OpenTypeKind.TabularType;         
      }
      #endregion
   }
}
