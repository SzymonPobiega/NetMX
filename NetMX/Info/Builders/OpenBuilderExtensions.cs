using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.OpenMBean;

namespace NetMX
{
   public static class OpenBuilderExtensions
   {
      public static Func<MBeanAttributeInfo> TypedAs(this IAttributeBuilder builder, OpenType openType)
      {
         ((IBuilder)builder).Descriptor.SetField(OpenTypeDescriptor.Field, openType);
         return builder.TypedAs(openType.Representation);
      }

      public static Func<MBeanParameterInfo> TypedAs(this IParameterBuilder builder, OpenType openType)
      {
         ((IBuilder)builder).Descriptor.SetField(OpenTypeDescriptor.Field, openType);
         return builder.TypedAs(openType.Representation);
      }

      public static IParameterBuilder WithDefaultValue(this IParameterBuilder builder, object defaultValue)
      {
         ((IBuilder)builder).Descriptor.SetField(DefaultValueDescriptor.Field, defaultValue);
         return builder;
      }

      public static IParameterBuilder WithMinimumValue(this IParameterBuilder builder, object minValue)
      {
         ((IBuilder)builder).Descriptor.SetField(MinValueDescriptor.Field, minValue);
         return builder;
      }

      public static IParameterBuilder WithMaximumValue(this IParameterBuilder builder, object maxValue)
      {
         ((IBuilder)builder).Descriptor.SetField(MaxValueDescriptor.Field, maxValue);
         return builder;
      }

      public static IParameterBuilder WithLimitedValues(this IParameterBuilder builder, IEnumerable<object> legalValues)
      {
         ((IBuilder)builder).Descriptor.SetField(LegalValuesDescriptor.Field, legalValues);
         return builder;
      }

      public static Func<MBeanOperationInfo> Returning(this IReturnTypeBuilder builder, OpenType openType)
      {
         ((IBuilder)builder).Descriptor.SetField(OpenTypeDescriptor.Field, openType);
         return builder.Returning(openType.Representation);
      }
   }
}