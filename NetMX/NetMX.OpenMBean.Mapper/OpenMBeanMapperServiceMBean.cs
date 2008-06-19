using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
	/// <summary>
	/// The Open MBean Mapper Service is responsible monitoring MBean registration and creating proxy OpenMBeans 
	/// for ordinary ones to expose their functionality to clients that are able only to interact with OpenMBeans.
	/// </summary>
   public interface OpenMBeanMapperServiceMBean
   {
		/// <summary>
		/// Gets or sets the ObjectName patterns that are applied to determine if an MBean should be proxied by
		/// this Mapper Service.
		/// </summary>
      ObjectName[] BeansToMapPatterns { get; set; }
		/// <summary>
		/// Unregisters all the proxy OpenMBeans and scans through all the registered MBeans to determine which are to
		/// be proxied and then proxies them.
		/// </summary>
      void RefreshMappings();
		/// <summary>
		/// Flushes the cache containing mappings of CLR types to creates <see cref="OpenType"/>s. Method is usefull
		/// after changing the <see cref="ITypeMapper"/> chain of responsibility.
		/// </summary>
      void FlushMappedTypeCache();
   }
}
