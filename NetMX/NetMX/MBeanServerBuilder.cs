#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX
{
	public abstract class MBeanServerBuilder : ProviderBaseEx
	{		
		public abstract IMBeanServer NewMBeanServer(string instanceName);	
	}
}
