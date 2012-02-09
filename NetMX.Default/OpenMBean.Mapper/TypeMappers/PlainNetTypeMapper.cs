using System;
using System.Collections.Generic;
using System.Reflection;
using NetMX.OpenMBean;
using NetMX.Server.OpenMBean.Mapper.Attributes;

namespace NetMX.Server.OpenMBean.Mapper.TypeMappers
{
   /// <summary>
   /// A mapper which maps the type to <see cref="CompositeType"/> and all its public readable properties
   /// to <see cref="CompositeType"/>'s items.
   /// </summary>
   public class PlainNetTypeMapper : ITypeMapper
   {
      #region ITypeMapper Members      
      public bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback)
      {
         mapsTo = OpenTypeKind.CompositeType;
         foreach (PropertyInfo propertyInfo in plainNetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
         {
            if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
            {
               OpenTypeKind featureMapsTo;
               if (!canHandleNestedTypeCallback(propertyInfo.PropertyType, out featureMapsTo))
               {
                  return false;
               }
            }
         }
         return true;
      }
      public OpenType MapType(Type plainNetType, MapTypeDelegate mapNestedTypeCallback)
      {
         List<string> names = new List<string>();
         List<string> descriptions = new List<string>();
         List<OpenType> types = new List<OpenType>();

         foreach (PropertyInfo propertyInfo in plainNetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
         {
            if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
            {
               names.Add(AttributeUtils.GetOpenTypeName(propertyInfo));
               descriptions.Add(AttributeUtils.GetOpenTypeDescription(propertyInfo));
               types.Add(mapNestedTypeCallback(propertyInfo.PropertyType));
            }
         }

         return new CompositeType(AttributeUtils.GetOpenTypeName(plainNetType), AttributeUtils.GetOpenTypeDescription(plainNetType), names, descriptions, types);
      }
      public object MapValue(Type plainNetType, OpenType mappedType, object value, MapValueDelegate mapNestedValueCallback)
      {
         if (value == null)
         {
            return null;
         }

         CompositeType compositeType = (CompositeType)mappedType;
         Type valueType = value.GetType();

         List<string> names = new List<string>();
         List<object> values = new List<object>();

         foreach (string itemName in compositeType.KeySet)
         {
            PropertyInfo propertyInfo = valueType.GetProperty(itemName, BindingFlags.Public | BindingFlags.Instance);
            object propValue = propertyInfo.GetValue(value, new object[] {});
            OpenType mappedPropertyType = compositeType.GetOpenType(itemName);
            values.Add(mapNestedValueCallback(propertyInfo.PropertyType, mappedPropertyType, propValue));
            names.Add(itemName);
         }
         return new CompositeDataSupport(compositeType, names, values);
      }
      #endregion
   }
}