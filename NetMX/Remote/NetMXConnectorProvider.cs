#region
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX.Remote
{
	public abstract class NetMXConnectorProvider : ProviderBaseEx
	{
		public abstract INetMXConnector NewNetMXConnector(Uri serviceUrl);
	}
}
