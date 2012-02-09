using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   public static class BuilderExtensions
   {
      public static IReturnTypeBuilder WithParameters(this IOperationBuilder builder, params Func<MBeanParameterInfo>[] parameters)
      {
         return builder.WithParameters(() => parameters.Select(x => x()));
      }

      public static IMBeanInfoBuilderOperations WithAttributes(this IMBeanInfoBuilderAttributes builder, params Func<MBeanAttributeInfo>[] attributes)
      {
         return builder.WithAttributes(() => attributes.Select(x => x()));
      }

      public static IMBeanInfoBuilderConstructors WithOperations(this IMBeanInfoBuilderOperations builder, params Func<MBeanOperationInfo>[] operations)
      {
         return builder.WithOperations(() => operations.Select(x => x()));
      }

      public static IMBeanInfoBuilderConstructors WithOperations(this IMBeanInfoBuilderOperations builder, params MBeanOperationInfo[] operations)
      {
         return builder.WithOperations(() => operations);
      }

      public static IMBeanInfoBuilderNotifications WithConstructors(this IMBeanInfoBuilderConstructors builder, params Func<MBeanConstructorInfo>[] constructors)
      {
         return builder.WithConstructors(() => constructors.Select(x => x()));
      }

      public static Func<MBeanInfo> WithNotifications(this IMBeanInfoBuilderNotifications builder, params Func<MBeanNotificationInfo>[] notifications)
      {
         return builder.WithNotifications(() => notifications.Select(x => x()));
      }

      public static Func<MBeanAttributeInfo> TypedAs<T>(this IAttributeBuilder builder)
      {
         return builder.TypedAs(typeof (T));
      }

      public static Func<MBeanParameterInfo> TypedAs<T>(this IParameterBuilder builder)
      {
         return builder.TypedAs(typeof (T));
      }

      public static Func<MBeanOperationInfo> Returning<T>(this IReturnTypeBuilder builder)
      {
         return builder.Returning(typeof (T));
      }

      
   }
}