#region
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
	public abstract class NetMXConnectorServerProvider : ProviderBase
	{
		public abstract INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server);
	}
}
