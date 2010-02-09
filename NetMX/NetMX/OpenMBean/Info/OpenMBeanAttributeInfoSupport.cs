#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

#endregion

namespace NetMX.OpenMBean
{
   
   /// <summary>
   /// Describes an attribute of an open MBean.
   /// </summary>
   [Serializable]   
   public class OpenMBeanAttributeInfoSupport : IOpenMBeanAttributeInfo
   {      
      private readonly MBeanAttributeInfo _wrappedInfo;

      /// <summary>
      /// Creates new <see cref="OpenMBeanAttributeInfoSupport"/> wrapping provided <see cref="MBeanAttributeInfo"/> value.
      /// </summary>
      /// <param name="wrappedInfo">Open MBean attribute metadata object to be wrappped.</param>
      public OpenMBeanAttributeInfoSupport(MBeanAttributeInfo wrappedInfo)
      {
         _wrappedInfo = wrappedInfo;
      }

      public string Type
      {
         get { return OpenType.Representation.AssemblyQualifiedName; }
      }

      public object DefaultValue
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(DefaultValueDescriptor.Field); }
      }

      public string Description
      {
         get { return _wrappedInfo.Description; }
      }

      public IEnumerable LegalValues
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(LegalValuesDescriptor.Field); }
      }

      public IComparable MaxValue
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(MaxValueDescriptor.Field); }
      }

      public IComparable MinValue
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(MinValueDescriptor.Field); }
      }

      public string Name
      {
         get { return _wrappedInfo.Name; }
      }

      public OpenType OpenType
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field); }
      }

      public bool HasDefaultValue
      {
         get { return _wrappedInfo.Descriptor.HasValue(DefaultValueDescriptor.Field); }
      }

      public bool HasLegalValues
      {
         get { return _wrappedInfo.Descriptor.HasValue(LegalValuesDescriptor.Field); }
      }

      public bool HasMaxValue
      {
         get { return _wrappedInfo.Descriptor.HasValue(MaxValueDescriptor.Field); }
      }

      public bool HasMinValue
      {
         get { return _wrappedInfo.Descriptor.HasValue(MinValueDescriptor.Field); }
      }

      public bool IsValue(object value)
      {
         return OpenType.IsValue(value);
      }

      public bool Readable
      {
         get { return _wrappedInfo.Readable; }
      }

      public bool Writable
      {
         get { return _wrappedInfo.Writable; }
      }
   }
}