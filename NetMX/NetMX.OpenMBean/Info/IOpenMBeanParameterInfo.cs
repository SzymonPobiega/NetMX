#region Using
using NetMX;
using System.Collections;
using System;
#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes a parameter used in one or more operations or constructors of an open MBean.
   /// 
   /// This interface declares the same methods as the class <see cref="MBeanParameterInfo"/>. A class 
   /// implementing this interface (typically OpenMBeanParameterInfoSupport) should extend
   /// <see cref="MBeanParameterInfo"/>.
   /// </summary>
   public interface IOpenMBeanParameterInfo
   {
      /// <summary>
      /// Gets the default value for this parameter, if it has one, or null otherwise.
      /// </summary>
      object DefaultValue { get; }
      /// <summary>
      /// Gets a human readable description of the parameter described by this OpenMBeanParameterInfo instance.
      /// </summary>
      string Description { get; }
      /// <summary>
      ///  Gets the set of legal values for this parameter, if it has one, or null otherwise.
      /// </summary>
      IEnumerable LegalValues { get; }
      /// <summary>
      /// Gets the maximal value for this parameter, if it has one, or null otherwise.
      /// </summary>
      IComparable MaxValue { get; }
      /// <summary>
      /// Gets the minimal value for this parameter, if it has one, or null otherwise.
      /// </summary>
      IComparable MinValue { get; }
      /// <summary>
      /// Gets the name of the parameter described by this OpenMBeanParameterInfo instance.
      /// </summary>
      string Name { get; }
      /// <summary>
      /// Gets the open type of the values of the parameter described by this OpenMBeanParameterInfo instance.
      /// </summary>
      OpenType OpenType { get; }
      /// <summary>
      /// Returns true if this parameter has a specified default value, or false otherwise.
      /// </summary>
      bool HasDefaultValue { get; }
      /// <summary>
      /// Returns true if this parameter has a specified set of legal values, or false otherwise.
      /// </summary>
      bool HasLegalValues { get; }
      /// <summary>
      /// Returns true if this parameter has a specified maximal value, or false otherwise.
      /// </summary>
      bool HasMaxValue { get; }
      /// <summary>
      /// Returns true if this parameter has a specified minimal value, or false otherwise.
      /// </summary>      
      bool HasMinValue { get; }
      /// <summary>
      /// Tests whether obj is a valid value for the parameter described by this OpenMBeanParameterInfo 
      /// instance.
      /// </summary>
      /// <param name="value">The object to be tested.</param>
      /// <returns>True if obj is a valid value for for the parameter described by this OpenMBeanParameterInfo instance, false otherwise.</returns>
      bool IsValue(object value);
   }
}