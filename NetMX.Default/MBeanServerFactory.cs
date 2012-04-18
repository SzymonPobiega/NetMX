using NetMX.Server;

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
