#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
#endregion

namespace NetMX
{
	public abstract class MBeanServerBuilder : ProviderBase
	{		
		public abstract IMBeanServer NewMBeanServer(string defaultDomain);	
	}
}
