#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes an attribute of an open MBean.
   /// 
   /// This interface declares the same methods as the class <see cref="MBeanAttributeInfo"/>. A class 
   /// implementing this interface (typically OpenMBeanAttributeInfoSupport) should extend
   /// <see cref="MBeanAttributeInfo"/>.
   /// </summary>
   public interface IOpenMBeanAttributeInfo : IOpenMBeanParameterInfo
   {
      /// <summary>
      /// Returns true if the attribute described by this IOpenMBeanAttributeInfo instance is readable, 
      /// false otherwise.
      /// </summary>
      bool Readable { get; }
      /// <summary>
      /// Returns true if the attribute described by this IOpenMBeanAttributeInfo instance is writable, 
      /// false otherwise.
      /// </summary>
      bool Writable { get; }
   }
}