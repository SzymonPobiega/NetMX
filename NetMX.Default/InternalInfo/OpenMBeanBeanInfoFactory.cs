using System;
using System.Linq;
using System.Reflection;
using NetMX.OpenMBean;


namespace NetMX.Server.InternalInfo
{
   internal sealed class OpenMBeanBeanInfoFactory : IMBeanInfoFactory
   {
      public MBeanInfo CreateMBeanInfo(Type intfType, System.Collections.Generic.IEnumerable<MBeanAttributeInfo> attributes, System.Collections.Generic.IEnumerable<MBeanConstructorInfo> constructors, System.Collections.Generic.IEnumerable<MBeanOperationInfo> operations, System.Collections.Generic.IEnumerable<MBeanNotificationInfo> notifications)
      {
         return new MBeanInfo(intfType.AssemblyQualifiedName,
                                         InfoUtils.GetDescrition(intfType, intfType, "Open MBean"),
                                         attributes,
                                         constructors,
                                         operations, notifications);
      }
      public MBeanAttributeInfo CreateMBeanAttributeInfo(PropertyInfo info)
      {
         Descriptor descriptor = new Descriptor();
         OpenType openType = OpenType.CreateOpenType(info.PropertyType);
         descriptor.SetField(OpenTypeDescriptor.Field, openType);
         object[] tmp = info.GetCustomAttributes(typeof(OpenMBeanAttributeAttribute), false);         
         if (tmp.Length > 0)
         {            
            OpenMBeanAttributeAttribute attr = (OpenMBeanAttributeAttribute)tmp[0];
            if (attr.LegalValues != null && (attr.MinValue != null || attr.MaxValue != null))
            {
               throw new OpenDataException("Cannot specify both min/max values and legal values.");
            }
            IComparable defaultValue = (IComparable)attr.DefaultValue;
            OpenInfoUtils.ValidateDefaultValue(openType, defaultValue);
            descriptor.SetField(DefaultValueDescriptor.Field, defaultValue);
            if (attr.LegalValues != null)
            {               
               OpenInfoUtils.ValidateLegalValues(openType, attr.LegalValues);
               descriptor.SetField(LegalValuesDescriptor.Field, attr.LegalValues);               
            }
            else
            {
               OpenInfoUtils.ValidateMinMaxValue(openType, defaultValue, attr.MinValue, attr.MaxValue);
               descriptor.SetField(MinValueDescriptor.Field, attr.MinValue);
               descriptor.SetField(MaxValueDescriptor.Field, attr.MaxValue);
            }            
         }
         return new MBeanAttributeInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean attribute"), openType.Representation.AssemblyQualifiedName,
            info.CanRead, info.CanWrite, descriptor);
      }
      public MBeanOperationInfo CreateMBeanOperationInfo(MethodInfo info)
      {
         Descriptor descriptor = new Descriptor();
         object[] attrTmp = info.GetCustomAttributes(typeof(OpenMBeanOperationAttribute), false);
         if (attrTmp.Length == 0)
         {
            throw new OpenDataException("Open MBean operation have to have its impact specified.");
         }
         OpenMBeanOperationAttribute attr = (OpenMBeanOperationAttribute)attrTmp[0];
         if (attr.Impact == OperationImpact.Unknown)
         {
            throw new OpenDataException("Open MBean operation have to have its impact specified.");
         }
         OpenType openType = info.ReturnType != null ? OpenType.CreateOpenType(info.ReturnType) : SimpleType.Void;
         descriptor.SetField(OpenTypeDescriptor.Field, openType);
         return new MBeanOperationInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean operation"),
                                       openType.Representation.AssemblyQualifiedName,
                                       info.GetParameters().Select(x => CreateMBeanParameterInfo(x)), attr.Impact,
                                       descriptor);
      }
      public MBeanConstructorInfo CreateMBeanConstructorInfo(ConstructorInfo info)
      {
         return new MBeanConstructorInfo(info.Name, InfoUtils.GetDescrition(info, info, "MBean constructor"),
            info.GetParameters().Select(x => CreateMBeanParameterInfo(x)));
      }      
      public MBeanNotificationInfo CreateMBeanNotificationInfo(EventInfo info, Type handlerType)
      {
         MBeanNotificationAttribute attribute =
            (MBeanNotificationAttribute)info.GetCustomAttributes(typeof(MBeanNotificationAttribute), true)[0];
         return new MBeanNotificationInfo(new[] { attribute.NotifType },
                                          handlerType.GetGenericArguments()[0].AssemblyQualifiedName,
                                          InfoUtils.GetDescrition(info, info, "MBean notification"));
      }
      private static MBeanParameterInfo CreateMBeanParameterInfo(ParameterInfo info)
      {
         Descriptor descriptor = new Descriptor();
         OpenType openType = OpenType.CreateOpenType(info.ParameterType);
         descriptor.SetField(OpenTypeDescriptor.Field, openType);
         object[] tmp = info.GetCustomAttributes(typeof(OpenMBeanAttributeAttribute), false);
         if (tmp.Length > 0)
         {
            OpenMBeanAttributeAttribute attr = (OpenMBeanAttributeAttribute)tmp[0];
            if (attr.LegalValues != null && (attr.MinValue != null || attr.MaxValue != null))
            {
               throw new OpenDataException("Cannot specify both min/max values and legal values.");
            }
            IComparable defaultValue = (IComparable)attr.DefaultValue;
            OpenInfoUtils.ValidateDefaultValue(openType, defaultValue);
            descriptor.SetField(DefaultValueDescriptor.Field, defaultValue);
            if (attr.LegalValues != null)
            {
               OpenInfoUtils.ValidateLegalValues(openType, attr.LegalValues);
               descriptor.SetField(LegalValuesDescriptor.Field, attr.LegalValues);               
            }
            else
            {
               OpenInfoUtils.ValidateMinMaxValue(openType, defaultValue, attr.MinValue, attr.MaxValue);
               descriptor.SetField(MinValueDescriptor.Field, attr.MinValue);
               descriptor.SetField(MaxValueDescriptor.Field, attr.MaxValue);
            }
         }
         return new MBeanParameterInfo(info.Name,
                                       InfoUtils.GetDescrition(info.Member, info, "MBean operation parameter", info.Name),
                                       openType.Representation.AssemblyQualifiedName, descriptor);
      }
   }
}