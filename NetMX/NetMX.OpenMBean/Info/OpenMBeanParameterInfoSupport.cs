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
   /// Describes a parameter used in one or more operations or constructors of an open MBean.
   /// </summary>
   [Serializable]   
   public class OpenMBeanParameterInfoSupport : MBeanParameterInfo, IOpenMBeanParameterInfo
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
      /// Constructs an OpenMBeanParameterInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the parameter.</param>
      /// <param name="description">A human readable description of the parameter.</param>
      /// <param name="openType">An open type of the parameter.</param>      
      public OpenMBeanParameterInfoSupport(string name, string description, OpenType openType)
         : base(name, description, openType.Representation.AssemblyQualifiedName)
      {
         if (openType == null)
         {
            throw new ArgumentNullException("openType");
         }
         _openType = openType;
      }
      /// <summary>
      /// Constructs an OpenMBeanParameterInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the parameter.</param>
      /// <param name="description">A human readable description of the parameter.</param>
      /// <param name="openType">An open type of the parameter.</param>      
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this parameter; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      public OpenMBeanParameterInfoSupport(string name, string description, OpenType openType, object defaultValue)
         : this(name, description, openType)
      {
         OpenInfoUtils.ValidateDefaultValue(openType, defaultValue);
         _defaultValue = defaultValue;
      }
      /// <summary>
      /// Constructs an OpenMBeanParameterInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the parameter.</param>
      /// <param name="description">A human readable description of the parameter.</param>
      /// <param name="openType">An open type of the parameter.</param>      
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this parameter; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      /// <param name="minValue">Minimum parameter value. Must be valid for the <paramref name="openType"/> specified for this 
      /// parameter; can be null, in which case it means that no minimal value is set.</param>
      /// <param name="maxValue">Maximum parameter value. Must be valid for the <paramref name="openType"/> 
      /// specified for this parameter; can be null, in which case it means that no minimal value is set.</param>
      public OpenMBeanParameterInfoSupport(string name, string description, OpenType openType, IComparable defaultValue, IComparable minValue, IComparable maxValue)
         : this(name, description, openType, defaultValue)
      {
         OpenInfoUtils.ValidateMinMaxValue(openType, defaultValue, minValue, maxValue);
         _minValue = minValue;
         _maxValue = maxValue;
      }      
      /// <summary>
      /// Constructs an OpenMBeanParameterInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the parameter.</param>
      /// <param name="description">A human readable description of the parameter.</param>
      /// <param name="openType">An open type of the parameter.</param>      
      /// <param name="defaultValue">Must be a valid value for the <paramref name="openType"/> specified for 
      /// this parameter; default value not supported for <see cref="ArrayType"/> and <see cref="TabularType"/>; 
      /// can be null, in which case it means that no default value is set.</param>
      /// <param name="legalValues">Legal values for this parameter. Each contained value must be valid for 
      /// the <paramref name="openType"/> specified for this parameter; legal values not supported for 
      /// <see cref="ArrayType"/> and <see cref="TabularType"/>; can be null or empty.</param>      
      public OpenMBeanParameterInfoSupport(string name, string description, OpenType openType, IComparable defaultValue, IEnumerable<object> legalValues)
         : this(name, description, openType, defaultValue)
      {
         OpenInfoUtils.ValidateLegalValues(openType, legalValues);
         _legalValues = new List<object>(legalValues).AsReadOnly();
      }      
      /// <summary>
      /// Constructs an OpenMBeanParameterInfoSupport object.
      /// </summary>
      /// <param name="info">Parameter information object.</param>
      public OpenMBeanParameterInfoSupport(ParameterInfo info)
			: base(info)
      {
         _openType = OpenMBean.OpenType.CreateFromType(info.ParameterType);
         object[] tmp = info.GetCustomAttributes(typeof (OpenMBeanParameterAttribute), false);
         if (tmp.Length > 0)
         {
            OpenMBeanParameterAttribute attr = (OpenMBeanParameterAttribute)tmp[0];
            if (attr.LegalValues != null && (attr.MinValue != null || attr.MaxValue != null))
            {
               throw new OpenDataException("Cannot specify both min/max values and legal values.");
            }            
            OpenInfoUtils.ValidateDefaultValue(_openType, attr.DefaultValue);
            IComparable defaultValue = (IComparable) attr.DefaultValue;
            IComparable minValue = (IComparable) attr.MinValue;
            IComparable maxValue = (IComparable) attr.MaxValue;
            OpenInfoUtils.ValidateMinMaxValue(_openType, defaultValue, minValue, maxValue);
            if (attr.LegalValues != null)
            {
               OpenInfoUtils.ValidateLegalValues(_openType, attr.LegalValues);
               _legalValues = new ReadOnlyCollection<object>(attr.LegalValues);
            }            
            _defaultValue = defaultValue;
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