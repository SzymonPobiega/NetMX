#region USING
using System;
using System.Collections.Generic;
using System.Text;
using NetMX.OpenMBean;

#endregion

namespace NetMX
{
    /// <summary>
    /// Defines the management interface of an object of class MBeanServerDelegate.
    /// </summary>
    [MBeanResource("NetMX.Resources.MBeanServerDelegate")]
    [OpenMBean]
    public interface MBeanServerDelegateMBean
    {
        /// <summary>
        /// Gets the NetMX implementation name (the name of this product).
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string ImplementationName { get; }
        /// <summary>
        /// Gets the NetMX implementation vendor (the vendor of this product).
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string ImplementationVendor { get; }
        /// <summary>
        /// Gets the NetMX implementation version (the version of this product).
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string ImplementationVersion { get; }
        /// <summary>
        /// Gets the MBean server agent identity.
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string MBeanServerId { get; }
        /// <summary>
        /// Gets the full name of the NetMX specification implemented by this product.
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string SpecificationName { get; }
        /// <summary>
        /// Gets the vendor of the JMX specification implemented by this product.
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string SpecificationVendor { get; }
        /// <summary>
        /// Gets the version of the JMX specification implemented by this product.
        /// </summary>
        [OpenMBeanAttributeAttribute]
        string SpecificationVersion { get; }
    }
}
