using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
   /// <summary>
   /// A mapper which maps "simple" types to <see cref="SimpleType"/> instances.
   /// </summary>
   public sealed class SimpleTypeMapper : ITypeMapper
   {
      #region ITypeMapper Members
      public bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback)
      {
         mapsTo = OpenTypeKind.SimpleType;
         return SimpleType.IsSimpleType(plainNetType);
      }
      public OpenType MapType(Type plainNetType, MapTypeDelegate mapNestedTypeCallback)
      {
         return SimpleType.CreateSimpleType(plainNetType);
      }
      public object MapValue(Type plainNetType, OpenType mappedType, object value, MapValueDelegate mapNestedValueCallback)
      {
         return value;
      }
      #endregion
   }
}
