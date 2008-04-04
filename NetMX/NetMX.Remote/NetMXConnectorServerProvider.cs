#region
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Simon.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
	public abstract class NetMXConnectorServerProvider : ProviderBaseEx
	{
		public abstract INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server);
	}
}
