#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Simon.Configuration.Provider;
#endregion

namespace NetMX
{
	public abstract class MBeanServerBuilder : ProviderBaseEx
	{		
		public abstract IMBeanServer NewMBeanServer(string defaultDomain);	
	}
}
