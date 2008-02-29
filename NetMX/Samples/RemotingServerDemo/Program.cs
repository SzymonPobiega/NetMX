using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Remote;

namespace RemotingServerDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
			Sample o = new Sample();
			ObjectName name = new ObjectName("Sample:");
			server.RegisterMBean(o, name);
			Uri serviceUrl = new Uri("tcp://localhost:1234/MBeanServer.tcp");

			using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
			{
				connectorServer.Start();
				Console.WriteLine("Press any key to quit");
				Console.ReadKey();
			}			
		}
	}
}
