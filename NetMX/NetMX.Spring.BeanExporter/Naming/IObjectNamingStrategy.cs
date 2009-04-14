using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter.Naming
{
   /// <summary>
   /// Represents strategy for giving NetMX names to Spring components being exported via NetMX.
   /// </summary>
   public interface IObjectNamingStrategy
   {
      /// <summary>
      /// Creates <see cref="ObjectName"/> for given bean instance and its container-level name.
      /// </summary>
      /// <param name="managedBean">Instance of object being exported.</param>
      /// <param name="beanKey">Spring container key.</param>
      /// <returns>NetMX name of exported object.</returns>
      ObjectName GetObjectName(Object managedBean, String beanKey);
   }
}
