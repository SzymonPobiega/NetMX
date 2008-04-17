#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
#endregion

namespace NetMX.Default.Configuration
{
	/// <summary>
	/// Configuration section for setting up MBean server instance.
	/// </summary>
	public class MBeanServerConfigurationSection : ConfigurationSection
	{		
		/// <summary>
		/// A collection of MBeans to be created on server start-up.
		/// </summary>
		[ConfigurationProperty("beans", IsRequired=true)]
		public MBeanCollection Beans
		{
			get { return (MBeanCollection)this["beans"]; }
			set { this["beans"] = value; }
		}
	}
}
