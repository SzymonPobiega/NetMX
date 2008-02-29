#region
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
#endregion

namespace NetMX.Remote
{
	public abstract class NetMXConnectorProvider : ProviderBase
	{
		public abstract INetMXConnector NewNetMXConnector(Uri serviceUrl);
	}
}
