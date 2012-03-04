namespace NetMX
{
    public static class MBeanServerConnectionExtensions
    {        
        public static dynamic CreateDynamicProxy(this IMBeanServerConnection connection, ObjectName name)
        {
            return new DynamicMBeanProxy(name, connection);
        }
    }
}