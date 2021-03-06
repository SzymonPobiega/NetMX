using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Remote;
using System.Security.Principal;
using NetMX.Remote.Jsr262;

namespace Jsr262Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectorServerFactory = new Jsr262ConnectorServerFactory();
            var connectorFactory = new Jsr262ConnectorFactory();

            IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
            Sample o = new Sample();
            ObjectName name = new ObjectName("Sample:a=b");
            server.RegisterMBean(o, name);
            Uri serviceUrl = new Uri("http://localhost:12345/MBeanServer");

            using (INetMXConnectorServer connectorServer = connectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
            {
                connectorServer.Start();

                using (INetMXConnector connector = connectorFactory.Connect(new Uri("http://localhost:12345/MBeanServer"), null))
                {
                    IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

                    string defaultDomain = remoteServer.GetDefaultDomain();
                    Console.WriteLine("Default domain is {0}", defaultDomain);

                    IEnumerable<string> domains = remoteServer.GetDomains();
                    Console.WriteLine("Following domains are registereds:");
                    foreach (string domain in domains)
                    {
                        Console.WriteLine(" * {0}", domain);
                    }

                    IEnumerable<ObjectName> names = remoteServer.QueryNames(null, new EqualExp(new AttributeExp("Counter"), new ConstantExp<Number>(0)));
                    Console.WriteLine("Following MBeans have attribute counter with value 0:");
                    foreach (ObjectName objectName in names)
                    {
                        Console.WriteLine(" * {0}", objectName);
                    }

                    //remoteServer.AddNotificationListener(name, CounterChanged, null, null);

                    Console.WriteLine("******");
                    MBeanInfo info = remoteServer.GetMBeanInfo(name);
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

                    object counter = remoteServer.GetAttribute(name, "Counter");

                    Console.WriteLine("Counter value is {0}", counter);

                    remoteServer.SetAttribute(name, "Counter", 5);
                    counter = remoteServer.GetAttribute(name, "Counter");

                    Console.WriteLine("Now, counter value is {0}", counter);

                    counter = remoteServer.Invoke(name, "AddAmount", new object[] { 5 });
                    counter = remoteServer.GetAttribute(name, "Counter");

                    Console.WriteLine("Now, counter value is {0}. Press <enter>", counter);
                    Console.ReadLine();

                    counter = remoteServer.Invoke(name, "ResetCounter", new object[] { });
                    counter = remoteServer.GetAttribute(name, "Counter");

                    Console.WriteLine("Now, counter value is {0}", counter);

                    Console.WriteLine("Press <enter> to exit.");
                    Console.ReadLine();
                }
            }
        }
        static void CounterChanged(Notification notification, object handback)
        {
            Console.WriteLine("Counter changed! New value is {0}", notification.UserData);
        }
    }
}
