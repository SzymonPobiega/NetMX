#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{   
   /// <summary>
   /// Defines the management interface of an object of class MBeanServerDelegate.
   /// </summary>
   [MBeanResource("NetMX.Resources.MBeanServerDelegate")]
   public interface MBeanServerDelegateMBean
   {
      /// <summary>
      /// Gets the NetMX implementation name (the name of this product).
      /// </summary>
      string ImplementationName { get; }
      /// <summary>
      /// Gets the NetMX implementation vendor (the vendor of this product).
      /// </summary>
      string ImplementationVendor { get; }
      /// <summary>
      /// Gets the NetMX implementation version (the version of this product).
      /// </summary>
      string ImplementationVersion { get; }
      /// <summary>
      /// Gets the MBean server agent identity.
      /// </summary>
      string MBeanServerId { get; }
      /// <summary>
      /// Gets the full name of the NetMX specification implemented by this product.
      /// </summary>
      string SpecificationName { get; }
      /// <summary>
      /// Gets the vendor of the JMX specification implemented by this product.
      /// </summary>
      string SpecificationVendor { get; }
      /// <summary>
      /// Gets the version of the JMX specification implemented by this product.
      /// </summary>
      string SpecificationVersion { get; }
   }
}
