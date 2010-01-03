#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace NetMX.Default.InternalInfo
{
   internal interface IMBeanInfoFactory
   {
      MBeanInfo CreateMBeanInfo(Type intfType, IEnumerable<MBeanAttributeInfo> attributes,
                                       IEnumerable<MBeanConstructorInfo> constructors,
                                       IEnumerable<MBeanOperationInfo> operations,
                                       IEnumerable<MBeanNotificationInfo> notifications);
      MBeanAttributeInfo CreateMBeanAttributeInfo(PropertyInfo info);
      MBeanOperationInfo CreateMBeanOperationInfo(MethodInfo info);
      MBeanConstructorInfo CreateMBeanConstructorInfo(ConstructorInfo info);
      MBeanNotificationInfo CreateMBeanNotificationInfo(EventInfo info, Type handlerType);
   }
}