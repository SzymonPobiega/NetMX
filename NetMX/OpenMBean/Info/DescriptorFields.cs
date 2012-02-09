using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Represents descriptor field containing open type value.
   /// </summary>
   public class OpenTypeDescriptor : DescriptorField<OpenType, OpenTypeDescriptor>
   {
      public OpenTypeDescriptor() 
         : base("openType")
      {
      }
   }
   /// <summary>
   /// Represents descriptor field containing attribute or parameter default value.
   /// </summary>
   public class DefaultValueDescriptor : DescriptorField<object, DefaultValueDescriptor>
   {
      public DefaultValueDescriptor()
         : base("defaultValue")
      {
      }
   }
   /// <summary>
   /// Represents descriptor field containing open type value.
   /// </summary>
   public class MinValueDescriptor : DescriptorField<IComparable, MinValueDescriptor>
   {
      public MinValueDescriptor()
         : base("minValue")
      {
      }
   }
   /// <summary>
   /// Represents descriptor field containing open type value.
   /// </summary>
   public class MaxValueDescriptor : DescriptorField<IComparable, MaxValueDescriptor>
   {
      public MaxValueDescriptor()
         : base("maxValue")
      {
      }
   }

   /// <summary>
   /// Represents descriptor field containing open type value.
   /// </summary>
   public class LegalValuesDescriptor : DescriptorField<IEnumerable<object>, LegalValuesDescriptor>
   {
      public LegalValuesDescriptor()
         : base("legalValues")
      {
      }
   }
}