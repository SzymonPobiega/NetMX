#region Using
using System;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Attribute which tells <see cref="IMBeanServer"/> implementation that decorated standard MBean interface
   /// conforms to OpenMBean specification.
   /// </summary>
   [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
   public sealed class OpenMBeanAttribute : Attribute
   {      
   }
}