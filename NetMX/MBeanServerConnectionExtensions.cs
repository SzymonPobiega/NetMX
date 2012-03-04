namespace NetMX
{
    ///<summary>
    /// Extension methods for <see cref="IMBeanServerConnection"/>.
    ///</summary>
    public static class MBeanServerConnectionExtensions
    {        
        ///<summary>
        /// Creates a new dynamic proxy object that forwards method invocations and property gets/sets to specified <see cref="IMBeanServerConnection"/>.
        ///</summary>
        ///<param name="connection">A connection to use.</param>
        ///<param name="name">An object name of MBean that this proxy applies to.</param>
        ///<returns></returns>
        public static dynamic CreateDynamicProxy(this IMBeanServerConnection connection, ObjectName name)
        {
            return new DynamicMBeanProxy(name, connection);
        }
    }
}