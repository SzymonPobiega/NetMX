#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote.Remoting
{
	public sealed class RemotingConnectorProvider : NetMXConnectorProvider
	{
		#region MEMBERS
		#endregion
		
		#region OVERRIDDEN        
		public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
		{
			return new RemotingConnector(serviceUrl);
		}
		#endregion
	}
}
