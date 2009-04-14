using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter
{
   public static class NetMXUtils
   {
      public static bool IsMBean(Type t)
      {
         return t != null &&
                (IsDynamicMBean(t) || HasMBeanInterface(t));
      }

      private static bool IsDynamicMBean(Type t)
      {
         return typeof (IDynamicMBean).IsAssignableFrom(t);
      }

      private static bool HasMBeanInterface(Type t)
      {
         string beanInterfaceName = t.Name + "MBean";
         return t.GetInterfaces().Any(x => x.Name == beanInterfaceName);
      }
   }
}
