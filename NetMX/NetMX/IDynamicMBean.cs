using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
    /// <summary>
    /// MBean that can change its structure over time. Can be used to instument dynamicaly changing
    /// resources. It is also used as internal representation of all kinds of MBeans
    /// </summary>
	public interface IDynamicMBean
	{
        /// <summary>
        /// Returns an object describing this MBean.
        /// </summary>
        /// <returns>Object describing this MBean</returns>
		MBeanInfo GetMBeanInfo();
        /// <summary>
        /// Gets value of a particular attribute of this MBean.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <returns>Value of the attribute</returns>
		object GetAttribute(string attributeName);
        /// <summary>
        /// Sets value of a particular attribute of this MBean.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
		void SetAttribute(string attributeName, object value);
        /// <summary>
        /// Executes a particular operation of this MBean and returns it's result.
        /// </summary>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="arguments">Operation arguments</param>
        /// <returns>Result of the operation</returns>
		object Invoke(string operationName, object[] arguments);
	}
}
