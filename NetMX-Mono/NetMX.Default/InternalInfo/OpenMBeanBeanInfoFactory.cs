#region Using
using System;
using System.Reflection;
using NetMX.OpenMBean;

#endregion

namespace NetMX.Default.InternalInfo
{
   internal sealed class OpenMBeanBeanInfoFactory : IMBeanInfoFactory
   {
      public MBeanInfo CreateMBeanInfo(Type intfType, System.Collections.Generic.IEnumerable<MBeanAttributeInfo> attributes, System.Collections.Generic.IEnumerable<MBeanConstructorInfo> constructors, System.Collections.Generic.IEnumerable<MBeanOperationInfo> operations, System.Collections.Generic.IEnumerable<MBeanNotificationInfo> notifications)
      {
         return new OpenMBeanInfoSupport(intfType, attributes, constructors, operations, notifications);
      }
      public MBeanAttributeInfo CreateMBeanAttributeInfo(PropertyInfo info)
      {
         return  new OpenMBeanAttributeInfoSupport(info);
      }
      public MBeanOperationInfo CreateMBeanOperationInfo(MethodInfo info)
      {
         return new OpenMBeanOperationInfoSupport(info);
      }
      public MBeanConstructorInfo CreateMBeanConstructorInfo(ConstructorInfo info)
      {
         return new OpenMBeanConstructorInfoSupport(info);
      }
      public MBeanNotificationInfo CreateMBeanNotificationInfo(EventInfo info, Type handlerType)
      {
         return new MBeanNotificationInfo(info, handlerType);
      }
   }
}