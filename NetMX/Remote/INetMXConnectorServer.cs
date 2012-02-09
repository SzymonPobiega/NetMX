#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX.Remote
{
	public interface INetMXConnectorServer : IDisposable
	{
		IMBeanServer MBeanServer { get; }		
		void Start();
		void Stop();		
	}
}
