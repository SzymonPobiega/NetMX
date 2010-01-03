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
   public class OpenMBeanAttributeInfoSupport : MBeanAttributeInfo, IOpenMBeanAttributeInfo
   {
      #region Members
      private readonly object _defaultValue;
      private readonly object _minValue;
      private readonly object _maxValue;
      private readonly ReadOnlyCollection<object> _legalValues;
      private readonly OpenType _openType;
      #endregion

      #region Constructors
      /// <summary>
      /// Constructs an MBeanAttributeInfo object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">A human readable description of the attribute.</param>
      /// <param name="openType">An open type of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
      public OpenMBeanAttributeInfoSupport(string name, string description, OpenType openType, bool isReadable, bool isWritable)
         : base(name, description, openType.Representation.AssemblyQualifiedName, isReadable, isWritable)
      {
         if (openType == null)
         {
            throw new ArgumentNullException("openType");
         }
         _openType = openType;
      }
      /// <summary>
      /// Constructs an MBeanAttributeInfo object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">A human readable description of the attribute.</param>
      /// <param name="openType">An open type of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this attribute; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      public OpenMBeanAttributeInfoSupport(string name, string description, OpenType openType, bool isReadable, bool isWritable, object defaultValue)
         : this(name, description, openType, isReadable, isWritable)
      {
         OpenInfoUtils.ValidateDefaultValue(openType, defaultValue);
         _defaultValue = defaultValue;
      }
      /// <summary>
      /// Constructs an OpenMBeanAttributeInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">A human readable description of the attribute.</param>
      /// <param name="openType">An open type of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this attribute; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      /// <param name="minValue">Minimum attribute value. Must be valid for the <paramref name="openType"/> specified for this 
      /// attribute; can be null, in which case it means that no minimal value is set.</param>
      /// <param name="maxValue">Maximum attribute value. Must be valid for the <paramref name="openType"/> 
      /// specified for this attribute; can be null, in which case it means that no minimal value is set.</param>
      public OpenMBeanAttributeInfoSupport(string name, string description, OpenType openType, bool isReadable, bool isWritable, IComparable defaultValue, IComparable minValue, IComparable maxValue)
         : this(name, description, openType, isReadable, isWritable, defaultValue)
      {
         OpenInfoUtils.ValidateMinMaxValue(openType, defaultValue, minValue, maxValue);
         _minValue = minValue;
         _maxValue = maxValue;
      }      
      /// <summary>
      /// Constructs an OpenMBeanAttributeInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">A human readable description of the attribute.</param>
      /// <param name="openType">An open type of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this attribute; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      /// <param name="legalValues">Legal values for this attribute. Each contained value must be valid for 
      /// the <paramref name="openType"/> specified for this attribute; legal values not supported for 
      /// <see cref="ArrayType"/> and <see cref="TabularType"/>; can be null or empty.</param>      
      public OpenMBeanAttributeInfoSupport(string name, string description, OpenType openType, bool isReadable, bool isWritable, IComparable defaultValue, IEnumerable<object> legalValues)
         : this(name, description, openType, isReadable, isWritable, defaultValue)
      {
         OpenInfoUtils.ValidateLegalValues(openType, legalValues);
         _legalValues = new List<object>(legalValues).AsReadOnly();
      }      
      /// <summary>
      /// Constructs an OpenMBeanAttributeInfoSupport object.
      /// </summary>
      /// <param name="info">Property information object.</param>
      public OpenMBeanAttributeInfoSupport(PropertyInfo info)
			: base(info)
      {
         _openType = OpenMBean.OpenType.CreateOpenType(info.PropertyType);
         object[] tmp = info.GetCustomAttributes(typeof (OpenMBeanAttributeAttribute), false);
         if (tmp.Length > 0)
         {
            OpenMBeanAttributeAttribute attr = (OpenMBeanAttributeAttribute) tmp[0];
            if (attr.LegalValues != null && (attr.MinValue != null || attr.MaxValue != null))
            {
               throw new OpenDataException("Cannot specify both min/max values and legal values.");
            }            
            OpenInfoUtils.ValidateDefaultValue(_openType, attr.DefaultValue);
            IComparable defaultValue = (IComparable)attr.DefaultValue;
            IComparable minValue = (IComparable)attr.MinValue;
            IComparable maxValue = (IComparable)attr.MaxValue;
            OpenInfoUtils.ValidateMinMaxValue(_openType, defaultValue, minValue, maxValue);
            if (attr.LegalValues != null)
            {
               OpenInfoUtils.ValidateLegalValues(_openType, attr.LegalValues);
               _legalValues = new ReadOnlyCollection<object>(attr.LegalValues);
            }            
            _defaultValue = attr.DefaultValue;
            _minValue = minValue;
            _maxValue = maxValue;  
         }
      }      
      #endregion


      #region IOpenMBeanParameterInfo Members
      public object DefaultValue
      {
         get { return _defaultValue; }
      }
      public IEnumerable LegalValues
      {
         get { return _legalValues; }
      }
      public IComparable MaxValue
      {
         get { return (IComparable)_maxValue; }
      }
      public IComparable MinValue
      {
         get { return (IComparable)_minValue; }
      }
      public OpenType OpenType
      {
         get { return _openType; }
      }
      public bool HasDefaultValue
      {
         get { return _defaultValue != null; }
      }
      public bool HasLegalValues
      {
         get { return _legalValues != null; }
      }
      public bool HasMaxValue
      {
         get { return _maxValue != null; }
      }
      public bool HasMinValue
      {
         get { return _minValue != null; }
      }
      public bool IsValue(object value)
      {
         return _openType.IsValue(value);
      }
      #endregion
   }
}