#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	public interface IMBeanServerConnection
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="callback"></param>
		/// <param name="filterCallback"></param>
		/// <param name="handback"></param>
		void AddNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="callback"></param>
		/// <param name="filterCallback"></param>
		/// <param name="handback"></param>
		void RemoveNotificationListener(ObjectName name, NotificationCallback callback, NotificationFilterCallback filterCallback, object handback);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="callback"></param>
		void RemoveNotificationListener(ObjectName name, NotificationCallback callback);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="operationName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		object Invoke(ObjectName name, string operationName, object[] arguments);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="attributeName"></param>
		/// <param name="value"></param>
		void SetAttribute(ObjectName name, string attributeName, object value);
		/// <summary>
		/// Gets the value of a specific attribute of a named MBean. The MBean is identified by its object name.
		/// </summary>
		/// <param name="name">The object name of the MBean from which the attribute is to be retrieved.</param>
		/// <param name="attributeName">A String specifying the name of the attribute to be retrieved.</param>
		/// <returns>The value of the retrieved attribute.</returns>
		object GetAttribute(ObjectName name, string attributeName);
		/// <summary>
		/// Enables the values of several attributes of a named MBean. The MBean is identified by its object name.
		/// If attribute cannot be found in MBean, it is not added on returned list. No exception is thrown in this
		/// case.
		/// </summary>
		/// <param name="name">The object name of the MBean from which the attributes are retrieved.</param>
		/// <param name="attributeNames">A list of the attributes to be retrieved.</param>
		/// <returns>The list of the retrieved attributes.</returns>
		/// <exception cref="NetMX.InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
		IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		MBeanInfo GetMBeanInfo(ObjectName name);
		/// <summary>
		/// Returns true if the MBean specified is an instance of the specified class, false otherwise.
		/// </summary>
		/// <param name="name">The <see cref="ObjectName"/> of the MBean.</param>
		/// <param name="className">The name of the class.</param>
		/// <returns>true if the MBean specified is an instance of the specified class, false otherwise.</returns>
		/// <exception cref="InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
		bool IsInstanceOf(ObjectName name, string className);
		/// <summary>
		/// Checks whether an MBean, identified by its object name, is already registered with the MBean server.
		/// </summary>
		/// <param name="name">The object name of the MBean to be checked.</param>
		/// <returns>True if the MBean is already registered in the MBean server, false otherwise.</returns>
		bool IsRegistered(ObjectName name);
		/// <summary>
		/// Unregisters an MBean from the MBean server. The MBean is identified by its object name. 
		/// Once the method has been invoked, the MBean may no longer be accessed by its object name.
		/// </summary>
		/// <param name="name">The object name of the MBean to be unregistered.</param>
		/// <exception cref="NetMX.InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
		/// <exception cref="NetMX.MBeanRegistrationException">The preDeregister method of the MBean has thrown an exception.</exception>
		void UnregisterMBean(ObjectName name);
	}
}
