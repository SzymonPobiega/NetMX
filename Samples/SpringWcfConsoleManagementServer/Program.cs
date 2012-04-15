using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.Relation;
using NetMX.Remote;
using Spring.Context;
using Spring.Context.Support;
using NetMX;

namespace WcfSpringConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IApplicationContext ctx = ContextRegistry.GetContext();


            using (INetMXConnector connector = NetMXConnectorFactory.Connect(new Uri("http://localhost:1010/MBeanServer"), null))
            {
                IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

                ObjectName name = "Sample:name=SampleComponent";

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

                object counter = remoteServer.GetAttribute(name, "Count");

                Console.WriteLine("Count value is {0}", counter);

                Console.WriteLine("Invoking StringAndIntOperation...");
                remoteServer.Invoke(name, "StringAndIntOperation", new object[] { "a", 5 });
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}