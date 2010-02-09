#region Using
using System;
using System.Linq;
using System.Reflection;

#endregion

namespace NetMX.Server.InternalInfo
{
   internal sealed class StandardBeanInfoFactory : IMBeanInfoFactory
   {
      public MBeanInfo CreateMBeanInfo(Type intfType, System.Collections.Generic.IEnumerable<MBeanAttributeInfo> attributes, System.Collections.Generic.IEnumerable<MBeanConstructorInfo> constructors, System.Collections.Generic.IEnumerable<MBeanOperationInfo> operations, System.Collections.Generic.IEnumerable<MBeanNotificationInfo> notifications)
      {
         return new MBeanInfo(intfType.AssemblyQualifiedName, InfoUtils.GetDescrition(intfType, intfType, "MBean"),
                              attributes, constructors, operations, notifications);
      }
      public MBeanAttributeInfo CreateMBeanAttributeInfo(PropertyInfo info)
      {
         return new MBeanAttributeInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean attribute"),
                                       info.PropertyType.AssemblyQualifiedName, info.CanRead, info.CanWrite);
      }
      public MBeanOperationInfo CreateMBeanOperationInfo(MethodInfo info)
      {
         return new MBeanOperationInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean operation"),
                                       info.ReturnType != null ? info.ReturnType.AssemblyQualifiedName : null,
                                       info.GetParameters().Select(x => CreateMBeanParameterInfo(x)),
                                       OperationImpact.Unknown);
      }
      public MBeanConstructorInfo CreateMBeanConstructorInfo(ConstructorInfo info)
      {
         return new MBeanConstructorInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean constructor"),
                                         info.GetParameters().Select(x => CreateMBeanParameterInfo(x)));
      }
      public MBeanNotificationInfo CreateMBeanNotificationInfo(EventInfo info, Type handlerType)
      {
         MBeanNotificationAttribute attribute =
            (MBeanNotificationAttribute) info.GetCustomAttributes(typeof (MBeanNotificationAttribute), true)[0];
         return new MBeanNotificationInfo(new[] {attribute.NotifType},
                                          handlerType.GetGenericArguments()[0].AssemblyQualifiedName,
                                          InfoUtils.GetDescrition(info, info, "MBean notification"));
      }

      private static MBeanParameterInfo CreateMBeanParameterInfo(ParameterInfo info)
      {
         return new MBeanParameterInfo(info.Name,
                                       InfoUtils.GetDescrition(info.Member, info, "MBean operation parameter", info.Name),
                                       info.ParameterType.AssemblyQualifiedName);
      }
   }
}