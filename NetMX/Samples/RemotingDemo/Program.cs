using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Remote;
using System.Security.Principal;

namespace RemotingDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
			Sample o = new Sample();
			ObjectName name = new ObjectName("Sample:");
			server.RegisterMBean(o, name);
			//Uri serviceUrl = new Uri("tcp://localhost:1234/MBeanServer.tcp");
         Uri serviceUrl = new Uri("http://localhost:1010/MBeanServer");      

			using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
			{
				connectorServer.Start();

				using (INetMXConnector connector = NetMXConnectorFactory.Connect(serviceUrl, null))
				{
					IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

					//remoteServer.AddNotificationListener(name, CounterChanged, null, null);

               //Console.WriteLine("******");
               //MBeanInfo info = remoteServer.GetMBeanInfo(name);
               //Console.WriteLine("MBean description: {0}", info.Description);
               //Console.WriteLine("MBean class name: {0}", info.ClassName);
               //foreach (MBeanAttributeInfo attributeInfo in info.Attributes)
               //{
               //   Console.WriteLine("Attribute {0} ({1}) [{2}{3}]: {4}", attributeInfo.Name, attributeInfo.Description,
               //      attributeInfo.Readable ? "r" : "", attributeInfo.Writable ? "w" : "", attributeInfo.Type);
               //}
               //foreach (MBeanOperationInfo operationInfo in info.Operations)
               //{
               //   Console.WriteLine("Operation {0} ({1}) [{2}]", operationInfo.Name, operationInfo.Description,
               //      operationInfo.Impact);
               //}
               //Console.WriteLine("******");

					object counter = remoteServer.GetAttribute(name, "Counter");

					Console.WriteLine("Counter value is {0}", counter);

               //remoteServer.SetAttribute(name, "Counter", 5);
               //counter = remoteServer.GetAttribute(name, "Counter");

               //Console.WriteLine("Now, counter value is {0}", counter);

               //counter = remoteServer.Invoke(name, "AddAmount", new object[] { 5 });
               //counter = remoteServer.GetAttribute(name, "Counter");

               //Console.WriteLine("Now, counter value is {0}", counter);

               //counter = remoteServer.Invoke(name, "ResetCounter", new object[] { });
               //counter = remoteServer.GetAttribute(name, "Counter");

               //Console.WriteLine("Now, counter value is {0}", counter);

					Console.ReadKey();
				}
			}
		}
		static void CounterChanged(Notification notification, object handback)
		{
			Console.WriteLine("Counter changed! New value is {0}", notification.UserData);
		}
	}
}
