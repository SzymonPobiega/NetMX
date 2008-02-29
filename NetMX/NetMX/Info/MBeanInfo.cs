#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
#endregion

namespace NetMX
{
    /// <summary>
    /// Describes the management interface exposed by an MBean; that is, the set of attributes and operations 
    /// which are available for management operations. Instances of this class are immutable. Subclasses may 
    /// be mutable but this is not recommended.
    /// </summary>
	[Serializable]    
	public class MBeanInfo
	{		
		#region PROPERTIES
		private string _description;
        /// <summary>
        /// Returns a human readable description of the MBean. 
        /// </summary>
		public string Description
		{
			get { return _description; }
		}
		private string _className;
        /// <summary>
        /// Returns the name of the .NET class of the MBean described by this MBeanInfo.
        /// </summary>
		public string ClassName
		{
			get { return _className; }
		}
        private ReadOnlyCollection<MBeanAttributeInfo> _attributes;
        /// <summary>
        /// Returns the list of attributes exposed for management. Each attribute is described by an 
        /// <see cref="NetMX.MBeanAttributeInfo"/> object. The returned list is read-only so it cannot be modified.
        /// List items are immutable by definition because they are of <see cref="NetMX.MBeanAttributeInfo"/> type.        
        /// </summary>
        public IList<MBeanAttributeInfo> Attributes
		{
			get { return _attributes; }
		}
        private ReadOnlyCollection<MBeanOperationInfo> _operations;
        /// <summary>
        /// Returns the list of operations exposed for management. Each operation is described by an 
        /// <see cref="NetMX.MBeanOperationInfo"/> object. The returned list is read-only so it cannot be modified.
        /// List items are immutable by definition because they are of <see cref="NetMX.MBeanOperationInfo"/> type.        
        /// </summary>
		public IList<MBeanOperationInfo> Operations
		{
			get { return _operations; }
		}
        private ReadOnlyCollection<MBeanNotificationInfo> _notifications;
        /// <summary>
        /// Returns the list of notifications exposed for management. Each operation is described by an 
        /// <see cref="NetMX.MBeanNotificationInfo"/> object. The returned list is read-only so it cannot be modified.
        /// List items are immutable by definition because they are of <see cref="NetMX.MBeanNotificationInfo"/> type.        
        /// </summary>
        public ReadOnlyCollection<MBeanNotificationInfo> Notifications
        {
            get { return _notifications; }
        }
		#endregion

		#region CONSTRUCTOR
        /// <summary>
        /// Constructs an <see cref="NetMX.MBeanInfo"/>.
        /// </summary>
        /// <param name="type">Type object of MBean implementation.</param>
        /// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
        /// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
        /// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
        public MBeanInfo(Type type, List<MBeanAttributeInfo> attributes, List<MBeanOperationInfo> operations, List<MBeanNotificationInfo> notifications)
            : this(type.AssemblyQualifiedName, InfoUtils.GetDescrition(type, "MBean"), attributes, operations, notifications)
        {            
        }
        /// <summary>
        /// Constructs an <see cref="NetMX.MBeanInfo"/>.
        /// </summary>
        /// <param name="className">Name of the MBean described by this MBeanInfo.</param>
        /// <param name="description">Human readable description of the MBean. </param>
        /// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
        /// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
        /// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
        public MBeanInfo(string className, string description, List<MBeanAttributeInfo> attributes, List<MBeanOperationInfo> operations, List<MBeanNotificationInfo> notifications)
		{
			_className = className;
			_description = description;
			_attributes = attributes.AsReadOnly();
			_operations = operations.AsReadOnly();
            _notifications = notifications.AsReadOnly();
		}
		#endregion
	}
}
