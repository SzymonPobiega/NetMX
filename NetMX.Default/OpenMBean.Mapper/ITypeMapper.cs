﻿using System;
using System.Collections.Generic;
using System.Text;
using NetMX.OpenMBean;

namespace NetMX.Server.OpenMBean.Mapper
{
   public delegate bool CanHandleDelegate(Type plainNetType,
                                          out OpenTypeKind mapsTo);

   public delegate OpenType MapTypeDelegate(Type plainNetType);

   public delegate object MapValueDelegate(Type plainNetType,
                                           OpenType mappedType, object value);

   public interface ITypeMapper
   {
      bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo, 
                     CanHandleDelegate canHandleNestedTypeCallback);
      OpenType MapType(Type plainNetType,
                       MapTypeDelegate mapNestedTypeCallback);
      object MapValue(Type plainNetType, OpenType mappedType, object value,
                      MapValueDelegate mapNestedValueCallback);
   }
}