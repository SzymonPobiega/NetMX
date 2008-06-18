using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
   public delegate OpenType MapTypeDelegate(Type plainNetType);
   public delegate bool CanHandleDelegate(Type plainNetType, out OpenTypeKind mapsTo);
   public delegate object MapValueDelegate(OpenType mappedType, object value);

   public interface ITypeMapper
   {
      bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, CanHandleDelegate canHandleNestedTypeCallback);
      OpenType MapType(Type plainNetType, MapTypeDelegate mapNestedTypeCallback);
      object MapValue(OpenType mappedType, object value, MapValueDelegate mapNestedValueCallback);
   }
}
