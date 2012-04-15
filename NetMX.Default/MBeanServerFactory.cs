#region USING
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Text;
using NetMX.Configuration.Provider;
using NetMX.Server;

#endregion

namespace NetMX
{
    public static class MBeanServerFactory
    {
        public static IMBeanServer CreateMBeanServer()
        {
            return new MBeanServer(null);
        }

        public static IMBeanServer CreateMBeanServer(string instanceName)
        {
            return new MBeanServer(instanceName);
        }
    }
}
