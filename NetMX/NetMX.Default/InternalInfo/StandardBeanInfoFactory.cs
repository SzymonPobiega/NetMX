#region Using
using System;
using System.Reflection;

#endregion

namespace NetMX.Server.InternalInfo
{
   internal sealed class StandardBeanInfoFactory : IMBeanInfoFactory
   {
      public MBeanInfo CreateMBeanInfo(Type intfType, System.Collections.Generic.IEnumerable<MBeanAttributeInfo> attributes, System.Collections.Generic.IEnumerable<MBeanConstructorInfo> constructors, System.Collections.Generic.IEnumerable<MBeanOperationInfo> operations, System.Collections.Generic.IEnumerable<MBeanNotificationInfo> notifications)
      {
         return new MBeanInfo(intfType, attributes, constructors, operations, notifications);
      }
      public MBeanAttributeInfo CreateMBeanAttributeInfo(PropertyInfo info)
      {
         return  new MBeanAttributeInfo(info);
      }
      public MBeanOperationInfo CreateMBeanOperationInfo(MethodInfo info)
      {
         return new MBeanOperationInfo(info);
      }
      public MBeanConstructorInfo CreateMBeanConstructorInfo(ConstructorInfo info)
      {
         return new MBeanConstructorInfo(info);
      }
      public MBeanNotificationInfo CreateMBeanNotificationInfo(EventInfo info, Type handlerType)
      {
         return new MBeanNotificationInfo(info, handlerType);
      }
   }
}