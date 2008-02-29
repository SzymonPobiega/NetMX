using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using System.Security.Permissions;

namespace SimpleConsoleDemo
{    
	class Program
	{                
        //static void TestPermission()
        //{
        //    MBeanCASPermission perm = new MBeanCASPermission("class", "member", new ObjectName("domain:"), MBeanPermissionAction.GetAttribute);
        //    perm.Demand();            
        //}    
        //[MBeanCASPermission(SecurityAction.Deny)]
		static void Main(string[] args)
		{
            //MBeanCASPermission perm = new MBeanCASPermission(null, null, null, MBeanPermissionAction.GetAttribute);
            //perm.Deny();
            //MBeanCASPermission perm2 = new MBeanCASPermission("class", "member", new ObjectName("domain:"), MBeanPermissionAction.GetAttribute);
            //perm.();
            //TestPermission();

			IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
			Sample o = new Sample();
			ObjectName name = new ObjectName("Sample:");
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

			object counter = server.GetAttribute(name, "Counter");

			Console.WriteLine("Counter value is {0}", counter);

			server.SetAttribute(name, "Counter", 5);
			counter = server.GetAttribute(name, "Counter");

			Console.WriteLine("Now, counter value is {0}", counter);
			
			counter = server.Invoke(name, "AddAmount", new object[] { 5 });
			counter = server.GetAttribute(name, "Counter");

			Console.WriteLine("Now, counter value is {0}", counter);

			counter = server.Invoke(name, "ResetCounter", new object[] { });
			counter = server.GetAttribute(name, "Counter");

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
