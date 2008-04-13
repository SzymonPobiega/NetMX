#region Using
using System.Collections.Generic;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes an operation of an Open MBean.
   /// 
   /// This interface declares the same methods as the class <see cref="MBeanOperationInfo"/>. A class 
   /// implementing this interface (typically OpenMBeanOperationInfoSupport) should extend 
   /// <see cref="MBeanOperationInfo"/>.
   /// 
   /// The <see cref="Signature"/> property should return at runtime an array of instances of a subclass of 
   /// <see cref="MBeanParameterInfo"/>  which implements the <see cref="IOpenMBeanParameterInfo"/> interface 
   /// (typically OpenMBeanParameterInfoSupport).
   /// </summary>
   public interface IOpenMBeanOperationInfo
   {
      /// <summary>
      /// Gets the name of the operation described by this IOpenMBeanOperationInfo instance.
      /// </summary>
      string Name { get; }
      /// <summary>
      /// Gets a human readable description of the operation described by this IOpenMBeanOperationInfo instance.
      /// </summary>
      string Description { get; }
      /// <summary>
      /// Gets an enum qualifying the impact of the operation described by this IOpenMBeanOperationInfo instance.
      /// </summary>
      OperationImpact Impact { get; }
      /// <summary>
      /// Gets the open type of the values returned by the operation described by this IOpenMBeanOperationInfo  instance.
      /// </summary>
      OpenType ReturnOpenType { get; }
      /// <summary>
      /// Gets the assembly-qualified class name of the values returned by the operation described by this 
      /// IOpenMBeanOperationInfo instance.
      /// </summary>
      string ReturnType { get; }
      /// <summary>
      /// Gets a list of <see cref="IOpenMBeanParameterInfo"/> instances describing each parameter in the 
      /// signature of the operation described by this IOpenMBeanOperationInfo instance. Each instance in the 
      /// returned list should actually be a subclass of <see cref="MBeanParameterInfo"/> which implements the 
      /// <see cref="IOpenMBeanParameterInfo"/> interface (typically OpenMBeanParameterInfoSupport).
      /// </summary>
      IList<MBeanParameterInfo> Signature { get; }
   }
}