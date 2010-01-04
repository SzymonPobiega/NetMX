using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NetMX.OpenMBean;

namespace NetMX.Server.OpenMBean.Mapper.TypeMappers
{
   /// <summary>
   /// A mapper which maps enumeration types.
   /// </summary>
   public sealed class EnumTypeMapper : ITypeMapper
   {
      #region Implementation of ITypeMapper
      public bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback)
      {
         mapsTo = OpenTypeKind.SimpleType;
         return plainNetType.IsEnum;
      }
      public OpenType MapType(Type plainNetType, MapTypeDelegate mapNestedTypeCallback)
      {
         return SimpleType.String;
      }
      public object MapValue(Type plainNetType, OpenType mappedType, object value, MapValueDelegate mapNestedValueCallback)
      {
         return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", value, Convert.ToUInt32(value));
      }
      #endregion
   }
}