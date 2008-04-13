#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   public abstract class OpenMBeanAttrParamBase : Attribute
   {
      private object _defaultValue;
      /// <summary>
      /// Gets or sets the default value for this parameter/attribute, if it has one, or null otherwise.
      /// </summary>
      public object DefaultValue
      {
         get { return _defaultValue; }
         set { _defaultValue = value; }
      }
      private object _minValue;
      /// <summary>
      /// Gets or sets the minimal value for this parameter/attribute, if it has one, or null otherwise.
      /// </summary>
      public object MinValue
      {
         get { return _minValue; }
         set { _minValue = value; }
      }      
      private object _maxValue;
      /// <summary>
      /// Gets or sets the maximal value for this parameter/attribute, if it has one, or null otherwise.
      /// </summary>
      public object MaxValue
      {
         get { return _maxValue; }
         set { _maxValue = value; }
      }
      private object[] _legalValues;
      /// <summary>
      /// Gets or sets the set of legal values for this parameter/attribute, if it has one, or null otherwise.
      /// </summary>
      public object[] LegalValues
      {
         get { return _legalValues; }
         set { _legalValues = value; }
      }      

      /// <summary>
      /// Creates new OpenMBeanAttrParamBase object.
      /// </summary>      
      protected OpenMBeanAttrParamBase()
      {         
      }
   }
}