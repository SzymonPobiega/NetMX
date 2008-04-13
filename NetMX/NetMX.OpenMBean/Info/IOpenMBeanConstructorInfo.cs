#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes a constructor of an Open MBean.
   /// 
   /// This interface declares the same methods as the class <see cref="MBeanConstructorInfo"/>. A class 
   /// implementing this interface (typically OpenMBeanConstructorInfoSupport) should extend 
   /// <see cref="MBeanConstructorInfo"/>.
   /// 
   /// The getSignature() method should return at runtime a list of instances of a subclass of 
   /// <see cref="MBeanParameterInfo"/> which implements the <see cref="IOpenMBeanParameterInfo"/> interface 
   /// (typically OpenMBeanParameterInfoSupport).
   /// </summary>
   public interface IOpenMBeanConstructorInfo
   {
      /// <summary>
      /// Gets the name of the constructor described by this IOpenMBeanConstructorInfo instance.
      /// </summary>
      string Name { get; }
      /// <summary>
      /// Gets a human readable description of the constructor described by this IOpenMBeanConstructorInfo 
      /// instance.
      /// </summary>
      string Description { get; }
      /// <summary>
      /// Gets a list of <see cref="IOpenMBeanParameterInfo"/> instances describing each parameter in the 
      /// signature of the constructor described by this IOpenMBeanConstructorInfo instance.
      /// </summary>
      IList<MBeanParameterInfo> Signature { get; }
   }
}