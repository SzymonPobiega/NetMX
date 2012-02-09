using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Represents base part of a descriptor field. Required for convenience of using fields. 
   /// This class should not be derived directly.
   /// </summary>
   /// <typeparam name="TValue">Type of field value.</typeparam>
   public abstract class DescriptorField<TValue>
   {
      private readonly string _name;

      protected DescriptorField(string name)
      {
         _name = name;
      }

      /// <summary>
      /// Gets name of this field used as descriptor key.
      /// </summary>
      public string Name
      {
         get { return _name; }
      }

      /// <summary>
      /// Checks whether provieded object would be a valid value for this field.
      /// Provided value cannot be null.
      /// </summary>
      /// <param name="value">Value candidate</param>
      /// <returns></returns>
      public virtual bool ValidateValue(object value)
      {
         if (value == null)
         {
            throw new ArgumentNullException("value");
         }
         return value is TValue;
      }
   }

   /// <summary>
   /// Base class for descriptor fields.
   /// </summary>
   /// <typeparam name="TValue">Type of field value.</typeparam>
   /// <typeparam name="TDescriptor">Reference to itself.</typeparam>
   public abstract class DescriptorField<TValue, TDescriptor> : DescriptorField<TValue>
      where TDescriptor : DescriptorField<TValue>, new()
   {
      private static readonly TDescriptor _singleValue = new TDescriptor();      

      /// <summary>
      /// Gets single instance of this descriptor field.
      /// </summary>
      public static TDescriptor Field
      {
         get { return _singleValue; }
      }

      protected DescriptorField(string name)
         : base(name)
      {
      }      
   }
}