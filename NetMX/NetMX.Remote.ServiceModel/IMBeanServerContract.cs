using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace NetMX.Remote.ServiceModel
{
   [ServiceContract]
   public interface IMBeanServerContract
   {      
      /// <summary>
      /// Adds a listener to a registered MBean. A notification emitted by an MBean will be forwarded by the 
      /// MBeanServer to the listener. If the source of the notification is a reference to an MBean object, 
      /// the MBean server will replace it by that MBean's ObjectName. Otherwise the source is unchanged.      
      /// </summary>
      /// <remarks>
      /// The listener object that receives notifications is the one that is registered with the given name at 
      /// the time this method is called. Even if it is subsequently unregistered, it will continue to receive 
      /// notifications.
      /// </remarks>
      /// <param name="name">The name of the MBean on which the listener should be added.</param>
      /// <param name="listener">The object name of the listener which will handle the notifications emitted by the registered MBean.</param>      
      [OperationContract]
      void AddNotificationListener(ObjectName name, ObjectName listener);      
      /// <summary>
      /// Instantiates and registers an MBean in the MBean server. An object name is associated to the MBean. 
      /// If the object name given is null, the MBean must provide its own name by implementing the 
      /// <see cref="NetMX.IMBeanRegistration"/> interface and returning the name from the <see cref="NetMX.IMBeanRegistration.PreRegister"/> method.
      /// </summary>
      /// <param name="className">The class name of the MBean to be instantiated.</param>
      /// <param name="name">The object name of the MBean. May be null.</param>
      /// <param name="arguments">An array containing the parameters of the constructor to be invoked.</param>
      /// <returns>An ObjectInstance, containing the ObjectName and the CLR class name of the newly 
      /// instantiated MBean. If the contained ObjectName is n, the contained CLR class name is 
      /// <see cref="IMBeanServer.GetMBeanInfo"/>(n).ClassName.</returns>
      [OperationContract]
      ObjectInstance CreateMBean(String className, ObjectName name, object[] arguments);            
      /// <summary>
      /// Removes a listener from a registered MBean. If the listener is registered more than once, perhaps with different filters or callbacks, 
      /// this method will remove all those registrations.
      /// </summary>
      /// <param name="name">The name of the MBean on which the listener should be removed.</param>
      /// <param name="listener">The object name of the listener to be removed.</param>
      /// <exception cref="NetMX.InstanceNotFoundException">The MBean name provided does not match any of the registered MBeans. </exception>
      /// <exception cref="NetMX.ListenerNotFoundException">The listener is not registered in the MBean.</exception>
      [OperationContract]
      void RemoveNotificationListener(ObjectName name, ObjectName listener);
      /// <summary>
      /// Invokes an operation on an MBean.
      /// </summary>
      /// <param name="name">The object name of the MBean on which the method is to be invoked.</param>
      /// <param name="operationName">The name of the operation to be invoked.</param>
      /// <param name="arguments">An array containing the parameters to be set when the operation is invoked.</param>
      /// <returns>The object returned by the operation, which represents the result of invoking the operation on the MBean specified.</returns>
      [OperationContract]
      object Invoke(ObjectName name, string operationName, object[] arguments);
      /// <summary>
      /// Sets the value of a specific attribute of a named MBean. The MBean is identified by its object name.
      /// </summary>
      /// <param name="name">The name of the MBean within which the attribute is to be set.</param>
      /// <param name="attributeName">The identification of the attribute to be set and the value it is to be set to</param>
      /// <param name="value">Value to set.</param>
      [OperationContract]
      void SetAttribute(ObjectName name, string attributeName, object value);
      /// <summary>
      /// Sets the values of several attributes of a named MBean. The MBean is identified by its object name.
      /// </summary>
      /// <param name="name">The object name of the MBean within which the attributes are to be set.</param>
      /// <param name="namesAndValues">A list of attributes: The identification of the attributes to be set and the values they are to be set to.</param>
      /// <exception cref="NetMX.InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
      [OperationContract]
      IList<AttributeValue> SetAttributes(ObjectName name, IEnumerable<AttributeValue> namesAndValues);
      /// <summary>
      /// Gets the value of a specific attribute of a named MBean. The MBean is identified by its object name.
      /// </summary>
      /// <param name="name">The object name of the MBean from which the attribute is to be retrieved.</param>
      /// <param name="attributeName">A String specifying the name of the attribute to be retrieved.</param>
      /// <returns>The value of the retrieved attribute.</returns>
      [OperationContract]
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
      [OperationContract]
      IList<AttributeValue> GetAttributes(ObjectName name, string[] attributeNames);
      /// <summary>
      /// Returns the number of MBeans registered in the MBean server.
      /// </summary>
      /// <returns>The number of MBeans registered.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [OperationContract]
      int GetMBeanCount();
      /// <summary>
      /// This method discovers the attributes and operations that an MBean exposes for management.
      /// </summary>
      /// <param name="name">The name of the MBean to analyze</param>
      /// <returns>An instance of <see cref="NetMX.MBeanInfo"/> allowing the retrieval of all attributes and operations of this MBean.</returns>
      [OperationContract]
      MBeanInfo GetMBeanInfo(ObjectName name);
      /// <summary>
      /// Returns true if the MBean specified is an instance of the specified class, false otherwise.
      /// </summary>
      /// <param name="name">The <see cref="ObjectName"/> of the MBean.</param>
      /// <param name="className">The name of the class.</param>
      /// <returns>true if the MBean specified is an instance of the specified class, false otherwise.</returns>
      /// <exception cref="InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
      [OperationContract]
      bool IsInstanceOf(ObjectName name, string className);
      /// <summary>
      /// Checks whether an MBean, identified by its object name, is already registered with the MBean server.
      /// </summary>
      /// <param name="name">The object name of the MBean to be checked.</param>
      /// <returns>True if the MBean is already registered in the MBean server, false otherwise.</returns>
      [OperationContract]
      bool IsRegistered(ObjectName name);
      /// <summary>
      /// Gets the names of MBeans controlled by the MBean server. This method enables any of the following to be 
      /// obtained: The names of all MBeans, the names of a set of MBeans specified by pattern matching on the 
      /// ObjectName and/or a Query expression, a specific MBean name (equivalent to testing whether an MBean 
      /// is registered). When the object name is null or no domain and key properties are specified, all objects 
      /// are selected (and filtered if a query is specified). It returns the set of ObjectNames for the MBeans 
      /// selected.
      /// </summary>
      /// <param name="name">The object name pattern identifying the MBean names to be retrieved. If null or no 
      /// domain and key properties are specified, the name of all registered MBeans will be retrieved.</param>
      /// <param name="query">The query expression to be applied for selecting MBeans. If null no query expression 
      /// will be applied for selecting MBeans.</param>
      /// <returns>A set containing the ObjectNames for the MBeans selected. If no MBean satisfies the query, an empty 
      /// list is returned.</returns>
      [OperationContract]
      IEnumerable<ObjectName> QueryNames(ObjectName name, QueryExp query);
      /// <summary>
      /// Unregisters an MBean from the MBean server. The MBean is identified by its object name. 
      /// Once the method has been invoked, the MBean may no longer be accessed by its object name.
      /// </summary>
      /// <param name="name">The object name of the MBean to be unregistered.</param>
      /// <exception cref="NetMX.InstanceNotFoundException">The MBean specified is not registered in the MBean server.</exception>
      /// <exception cref="NetMX.MBeanRegistrationException">The preDeregister method of the MBean has thrown an exception.</exception>
      [OperationContract]
      void UnregisterMBean(ObjectName name);
      /// <summary>
      /// Returns the default domain used for naming the MBean. The default domain name is used as the domain 
      /// part in the ObjectName of MBeans if no domain is specified by the user.
      /// </summary>
      /// <returns>The default domain.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [OperationContract]
      string GetDefaultDomain();
      /// <summary>
      /// Returns the list of domains in which any MBean is currently registered. A string is in the returned 
      /// list if and only if there is at least one MBean registered with an ObjectName whose GetDomain() is 
      /// equal to that string. The order of strings within the returned list is not defined.
      /// </summary>
      /// <returns>The list of domains.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [OperationContract]
      IList<string> GetDomains();
   }
}
