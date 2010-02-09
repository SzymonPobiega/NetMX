using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Contains extension methods for checking whether proviede MBean info is of open MBean and obtaining <see cref="IOpenMBeanInfo"/>
   /// form <see cref="MBeanInfo"/>.
   /// </summary>
   public static class OpenMBeanInfoExtensions
   {
      /// <summary>
      /// Checks whether provided MBean info is of open MBean.
      /// </summary>
      /// <param name="info">MBean metadata.</param>
      /// <returns></returns>
      public static bool IsOpen(this MBeanInfo info)
      {
         return info.Attributes.All(x => x.IsOpen()) &&
                info.Operations.All(x => x.IsOpen()) &&
                info.Constructors.All(x => x.IsOpen());         
      }

      private static bool IsOpen(this MBeanFeatureInfo featureInfo)
      {
         return featureInfo.Descriptor.HasValue(OpenTypeDescriptor.Field);
      }

      /// <summary>
      /// Casts provided MBean metadata to open MBean metadata object. Throws if proviede metadata is not of open MBean.
      /// </summary>
      /// <param name="info">Open MBean metadata.</param>
      /// <returns></returns>
      public static IOpenMBeanInfo AsOpen(this MBeanInfo info)
      {
         if (!IsOpen(info))
         {
            throw new InvalidOperationException("This is not an open MBean info.");
         }
         return new OpenMBeanInfoSupport(info);
      }

      /// <summary>
      /// If provided MBean metadata is of open MBean, returns this metadata in form of <see cref="IOpenMBeanInfo"/>. Otherwise
      /// returns null.
      /// </summary>
      /// <param name="info">Possibly open MBean metadata.</param>
      /// <returns></returns>
      public static IOpenMBeanInfo TryAsOpen(this MBeanInfo info)
      {
         if (!IsOpen(info))
         {
            return null;
         }
         return new OpenMBeanInfoSupport(info);
      }
   }
}