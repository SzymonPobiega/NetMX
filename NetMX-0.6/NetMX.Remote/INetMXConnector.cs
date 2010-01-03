using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Remote
{
	public interface INetMXConnector : IDisposable
	{
		void Close();
		void Connect(object credentials);
		string ConnectionId { get; }
		IMBeanServerConnection MBeanServerConnection { get; }
	}
}
