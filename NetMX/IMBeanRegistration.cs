using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
    /// <summary>
    /// Can be implemented by an MBean in order to carry out operations before and after being registered 
    /// or unregistered from the MBean server. 
    /// </summary>
    public interface IMBeanRegistration
    {
        /// <summary>
        /// Allows the MBean to perform any operations needed after having been unregistered in the MBean server. 
        /// </summary>
        void PostDeregister();
        /// <summary>
        /// Allows the MBean to perform any operations needed after having been registered in the MBean server 
        /// or after the registration has failed. 
        /// </summary>
        /// <param name="registrationDone">Indicates whether or not the MBean has been successfully registered 
        /// in the MBean server. The value false means that the registration phase has failed.</param>
        void PostRegister(bool registrationDone);
        /// <summary>
        /// Allows the MBean to perform any operations it needs before being unregistered by the MBean server.
        /// </summary>
        void PreDeregister();
        /// <summary>
        /// Allows the MBean to perform any operations it needs before being registered in the MBean server. 
        /// If the name of the MBean is not specified, the MBean can provide a name for its registration. 
        /// If any exception is raised, the MBean will not be registered in the MBean server. 
        /// </summary>
        /// <param name="server">The MBean server in which the MBean will be registered.</param>
        /// <param name="name">The object name of the MBean. This name is null if the name parameter to one 
        /// of the createMBean or registerMBean methods in the MBeanServer interface is null.</param>
        /// <returns>The name under which the MBean is to be registered. This value must not be null. 
        /// If the name parameter is not null, it will usually but not necessarily be the returned value.</returns>
        ObjectName PreRegister(IMBeanServer server, ObjectName name);
    }
}
