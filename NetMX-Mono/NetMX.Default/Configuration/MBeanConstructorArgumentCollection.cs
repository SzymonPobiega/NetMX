#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
#endregion

namespace NetMX.Default.Configuration
{
	/// <summary>
	/// Represents a collection of MBean constructor arguments. Arguments in the collection must be placed in the
	/// same order as in the constructor signature.
	/// </summary>
	public class MBeanConstructorArgumentCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{			
			return new MBeanConstructorArgument();
		}
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MBeanConstructorArgument)element).Name;	
		}		
	}
}
