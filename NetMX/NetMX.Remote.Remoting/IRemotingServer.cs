using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote.Remoting
{
	public interface IRemotingServer
	{
		IRemotingConnection NewClient(object credentials);
	}
}
