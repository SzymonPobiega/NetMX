using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper.Tests
{
   public abstract class MapperTestBase
   {
      protected abstract ITypeMapper Mapper { get; }

      protected virtual OpenType MapType(Type plainNetType)
      {
         if (plainNetType == typeof(int))
         {
            return SimpleType.Integer;
         }
         if (plainNetType == typeof(string))
         {
            return SimpleType.String;
         }
         return Mapper.MapType(plainNetType, MapType);
      }
      protected virtual bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo)
      {
         mapsTo = OpenTypeKind.SimpleType;
         if (plainNetType == typeof(int) || plainNetType == typeof(string))
         {
            mapsTo = OpenTypeKind.SimpleType;
            return true;
         }
         return Mapper.CanHandle(plainNetType, out mapsTo, CanHandle);
      }
      protected virtual object MapValue(OpenType mappedType, object value)
      {
         if (mappedType.Kind == OpenTypeKind.SimpleType)
         {
            return value;
         }
         return Mapper.MapValue(mappedType, value, MapValue);
      }
   }
}
