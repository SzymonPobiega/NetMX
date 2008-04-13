#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The OpenMBeanInfoSupport class describes the management information of an open MBean: it is a subclass 
   /// of <see cref="MBeanInfo"/>, and it implements the <see cref="IOpenMBeanInfo"/> interface. Note that an 
   /// open MBean is recognized as such if its <see cref="IDynamicMBean.GetMBeanInfo"/> method returns an 
   /// instance of a class which implements the <see cref="IOpenMBeanInfo"/> interface, typically 
   /// IOpenMBeanInfoSupport.
   /// </summary>
   [Serializable]
   public class OpenMBeanInfoSupport : MBeanInfo, IOpenMBeanInfo
   {      
		/// <summary>
		/// Constructs
		/// </summary>
		/// <param name="type">Type object of MBean implementation.</param>
		/// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
      /// <param name="constructors">List of MBean constructors. It should be an empty list if MBean contains no constructors.</param>
		/// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
		/// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
		public OpenMBeanInfoSupport(Type type, IEnumerable<IOpenMBeanAttributeInfo> attributes, IEnumerable<IOpenMBeanConstructorInfo> constructors, IEnumerable<IOpenMBeanOperationInfo> operations, IEnumerable<MBeanNotificationInfo> notifications)
			: base(type,
         OpenInfoUtils.Transform<MBeanAttributeInfo, IOpenMBeanAttributeInfo>(attributes),
         OpenInfoUtils.Transform<MBeanConstructorInfo, IOpenMBeanConstructorInfo>(constructors),
         OpenInfoUtils.Transform<MBeanOperationInfo, IOpenMBeanOperationInfo>(operations),
         new List<MBeanNotificationInfo>(notifications).AsReadOnly(),
         true)
		{
		}
		/// <summary>
		/// Constructs
		/// </summary>
		/// <param name="className">Name of the MBean described by this MBeanInfo.</param>
		/// <param name="description">Human readable description of the MBean. </param>
		/// <param name="attributes">List of MBean attributes. It should be an empty list if MBean contains no attributes.</param>
      /// <param name="constructors">List of MBean constructors. It should be an empty list if MBean contains no constructors.</param>
		/// <param name="operations">List of MBean operations. It should be an empty list if MBean contains no operations.</param>
		/// <param name="notifications">List of MBean notifications. It should be an empty list if MBean contains no notifications.</param>
      public OpenMBeanInfoSupport(string className, string description, IEnumerable<IOpenMBeanAttributeInfo> attributes, IEnumerable<IOpenMBeanConstructorInfo> constructors, IEnumerable<IOpenMBeanOperationInfo> operations, IEnumerable<MBeanNotificationInfo> notifications)
         : base(className, description,
         OpenInfoUtils.Transform<MBeanAttributeInfo, IOpenMBeanAttributeInfo>(attributes),
         OpenInfoUtils.Transform<MBeanConstructorInfo, IOpenMBeanConstructorInfo>(constructors),
         OpenInfoUtils.Transform<MBeanOperationInfo, IOpenMBeanOperationInfo>(operations),
         new List<MBeanNotificationInfo>(notifications).AsReadOnly(),
         true)
		{         
		}      
   }
}