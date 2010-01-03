#region USING
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Text;
using NetMX.Configuration.Provider;
using Simon.Configuration;

#endregion

namespace NetMX
{
   [ConfigurationSection("netMX", DefaultProvider = true)]
   public sealed class MBeanServerFactory : ServiceBase<MBeanServerBuilder>
   {
      private static readonly MBeanServerFactory _instance = new MBeanServerFactory();

      public static IMBeanServer CreateMBeanServer()
      {
         return _instance.Default.NewMBeanServer(null);
      }
      public static IMBeanServer CreateMBeanServer(string instanceName)
      {
         return _instance.Default.NewMBeanServer(instanceName);
      }
   }
}
