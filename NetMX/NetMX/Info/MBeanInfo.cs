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
		private readonly string _description;
		/// <summary>
		/// Returns a human readable description of the MBean. 
		/// </summary>
		public string Description
		{
			get { return _description; }
		}
      private readonly string _className;
		/// <summary>
		/// Returns the name of the .NET class of the MBean described by this MBeanInfo.
		/// </summary>
		public string ClassName
		{
			get { return _className; }
		}
	   private readonly Descriptor _descriptor;
      /// <summary>
      /// Gets descriptor of this feature.
      /// </summary>
      public Descriptor Descriptor
      {
         get { return _descriptor; }
      }
      private readonly ReadOnlyCollection<MBeanAttributeInfo> _attributes;
		/// <summary>
		/// Returns the list of attributes exposed for management. Each attribute is described by an 
		/// <see cref="NetMX.MBeanAttributeInfo"/> object. The returned list is read-only so it cannot be modified.
		/// List items are immutable by definition because they are of <see cref="NetMX.MBeanAttributeInfo"/> type.        
		/// </summary>
		public IList<MBeanAttributeInfo> Attributes
		{
			get { return _attributes; }
		}
      private readonly ReadOnlyCollection<MBeanOperationInfo> _operations;
		/// <summary>
		/// Returns the list of operations exposed for management. Each operation is described by an 
		/// <see cref="NetMX.MBeanOperationInfo"/> object. The returned list is read-only so it cannot be modified.
		/// List items are immutable by definition because they are of <see cref="NetMX.MBeanOperationInfo"/> type.        
		/// </summary>
		public IList<MBeanOperationInfo> Operations
		{
			get { return _operations; }
		}
      private readonly ReadOnlyCollection<MBeanNotificationInfo> _notifications;
		/// <summary>
		/// Returns the list of notifications exposed for management. Each operation is described by an 
		/// <see cref="NetMX.MBeanNotificationInfo"/> object. The returned list is read-only so it cannot be modified.
		/// List items are immutable by definition because they are of <see cref="NetMX.MBeanNotificationInfo"/> type.        
		/// </summary>
		public IList<MBeanNotificationInfo> Notifications
		{
			get { return _notifications; }
		}
      private readonly ReadOnlyCollection<MBeanConstructorInfo> _constructors;
      /// <summary>
      /// Returns the list of the public constructors of the MBean. Each constructor is described by an 
      /// <see cref="NetMX.MBeanConstructorInfo"/> object. The returned list is read-only so it cannot be modified.            
      /// </summary>
      /// <remarks>
      /// The returned list is not necessarily exhaustive. That is, the MBean may have a public constructor that 
      /// is not in the list. In this case, the MBean server can construct another instance of this MBean's class 
      /// using that constructor, even though it is not listed here.
      ///</remarks>            
      public IList<MBeanConstructorInfo> Constructors
      {
         get { return _constructors; }
      }
		#endregion

		#region CONSTRUCTOR		
		/// <summary>
		/// Constructs an <see cref="NetMX.MBeanInfo"/>.
		/// </summary>
		/// <param name="className">Name of the MBean described by this MBeanInfo.</param>
		/// <param name="description">Human readable description of the MBean. </param>
		/// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
      /// <param name="constructors">List of MBean constructors. It should be an empty list if MBean contains no constructors.</param>
		/// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
		/// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
      /// <param name="descriptor">Initial descriptor values.</param>
      public MBeanInfo(string className, string description, IEnumerable<MBeanAttributeInfo> attributes, IEnumerable<MBeanConstructorInfo> constructors, IEnumerable<MBeanOperationInfo> operations, IEnumerable<MBeanNotificationInfo> notifications, Descriptor descriptor)
		{
			_className = className;
			_description = description;
		   _attributes = new List<MBeanAttributeInfo>(attributes).AsReadOnly();
         _constructors = new List<MBeanConstructorInfo>(constructors).AsReadOnly();
			_operations = new List<MBeanOperationInfo>(operations).AsReadOnly();
			_notifications = new List<MBeanNotificationInfo>(notifications).AsReadOnly();
		   _descriptor = descriptor;
		}
      /// <summary>
      /// Constructs an <see cref="NetMX.MBeanInfo"/>.
      /// </summary>
      /// <param name="className">Name of the MBean described by this MBeanInfo.</param>
      /// <param name="description">Human readable description of the MBean. </param>
      /// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
      /// <param name="constructors">List of MBean constructors. It should be an empty list if MBean contains no constructors.</param>
      /// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
      /// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
      public MBeanInfo(string className, string description, IEnumerable<MBeanAttributeInfo> attributes, IEnumerable<MBeanConstructorInfo> constructors, IEnumerable<MBeanOperationInfo> operations, IEnumerable<MBeanNotificationInfo> notifications)
         : this(className, description, attributes, constructors, operations, notifications,new Descriptor())
      {         
      } 
		#endregion
	}
}
