using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using System.Security.Permissions;

namespace SimpleConsoleDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
			Counter o = new Counter();
			ObjectName name = new ObjectName("QuickStart:type=counter");
			server.RegisterMBean(o, name);

			Console.WriteLine("******");
			MBeanInfo info = server.GetMBeanInfo(name);
			Console.WriteLine("MBean description: {0}", info.Description);
			Console.WriteLine("MBean class name: {0}", info.ClassName);
			foreach (MBeanAttributeInfo attributeInfo in info.Attributes)
			{
				Console.WriteLine("Attribute {0} ({1}) [{2}{3}]: {4}", attributeInfo.Name, attributeInfo.Description,
					attributeInfo.Readable ? "r" : "", attributeInfo.Writable ? "w" : "", attributeInfo.Type);
			}
			foreach (MBeanOperationInfo operationInfo in info.Operations)
			{
				Console.WriteLine("Operation {0} ({1}) [{2}]", operationInfo.Name, operationInfo.Description,
					operationInfo.Impact);
			}
			Console.WriteLine("******");

			server.AddNotificationListener(name, CounterChanged, null, null);

			object counter = server.GetAttribute(name, "Value");

			Console.WriteLine("Counter value is {0}", counter);

			server.SetAttribute(name, "Value", 5);
			counter = server.GetAttribute(name, "Value");

			Console.WriteLine("Now, counter value is {0}", counter);

			counter = server.Invoke(name, "Add", new object[] { 5 });
			counter = server.GetAttribute(name, "Value");

			Console.WriteLine("Now, counter value is {0}", counter);

			counter = server.Invoke(name, "Reset", new object[] { });
			counter = server.GetAttribute(name, "Value");

			Console.WriteLine("Now, counter value is {0}", counter);

			server.RemoveNotificationListener(name, CounterChanged, null, null);

			Console.ReadKey();
		}
		static void CounterChanged(Notification notification, object handback)
		{
			Console.WriteLine("Counter changed! New value is {0}", notification.UserData);
		}
	}
}
