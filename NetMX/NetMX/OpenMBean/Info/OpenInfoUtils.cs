#region USING
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
#endregion

namespace NetMX.OpenMBean
{
   internal static class OpenInfoUtils
   {
      internal static IEnumerable<TSource> ValidateAs<TSource,TDest>(IEnumerable<TSource> source)
      {
         foreach (TSource o in source)
         {
            if (!(o is TDest))
            {
               throw new ArgumentException("Invalid argument type. Should be "+typeof(TDest).AssemblyQualifiedName);
            }
         }
         return source;
      }
      internal static ReadOnlyCollection<TDest> Transform<TDest, TSource>(IEnumerable<TSource> source)
      {
         List<TDest> results = new List<TDest>();
         foreach (TSource element in source)
         {
            results.Add((TDest)(object)element);
         }
         return results.AsReadOnly();
      }
      internal static void ValidateDefaultValue(OpenType openType, object defaultValue)
      {
         if (defaultValue != null)
         {
            if (!openType.IsValue(defaultValue))
            {
               throw new OpenDataException("Default value must be valid for supplied open type.");
            }
            if (openType.Kind == OpenTypeKind.ArrayType || openType.Kind == OpenTypeKind.TabularType)
            {
               throw new OpenDataException("Cannot specify default value for attribute of type array or tabular.");
            }
         }
      }
      internal static void ValidateMinMaxValue(OpenType openType, IComparable defaultValue, IComparable minValue, IComparable maxValue)
      {
         if (minValue != null)
         {
            if (!openType.IsValue(minValue))
            {
               throw new OpenDataException("Minimum value must be valid for supplied open type.");
            }
            if (defaultValue != null && minValue.CompareTo(defaultValue) > 0)
            {
               throw new OpenDataException("Minimum value must be less or equal to default value.");
            }
         }
         if (maxValue != null)
         {
            if (!openType.IsValue(maxValue))
            {
               throw new OpenDataException("Maximum value must be valid for supplied open type.");
            }
            if (defaultValue != null && defaultValue.CompareTo(maxValue) > 0)
            {
               throw new OpenDataException("Maximum value must be greater than or equal to default value.");
            }
         }
         if (maxValue != null && minValue != null && minValue.CompareTo(maxValue) > 0)
         {
            throw new OpenDataException("Maximum value must be greater than or equal to minimum value.");
         }
      }
      internal static void ValidateLegalValues(OpenType openType, IEnumerable<object> legalValues)
      {
         if (legalValues == null)
         {
            throw new ArgumentNullException("legalValues");
         }
         if (openType.Kind == OpenTypeKind.ArrayType || openType.Kind == OpenTypeKind.TabularType)
         {
            throw new OpenDataException("Cannot specify legal values for attribute of type array or tabular.");
         }
         foreach (object o in legalValues)
         {
            if (!openType.IsValue(o))
            {
               throw new OpenDataException("Each legal value must be valid for supplied open type.");
            }
         }
      }
   }
}
