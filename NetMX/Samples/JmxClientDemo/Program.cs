using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using NetMX;
using NetMX.Remote;

namespace JmxClientDemo
{
   class Program
   {
      static void Main(string[] args)
      {
         AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

         using (INetMXConnector connector = NetMXConnectorFactory.Connect(new Uri("http://localhost:9998/jmxws"), null))
         {
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;
            ObjectName name = ":type=Sample2";
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

            object counter = remoteServer.GetAttribute(name, "Value");

            Console.WriteLine("Counter value is {0}", counter);

            remoteServer.SetAttribute(name, "Value", 5);
            counter = remoteServer.GetAttribute(name, "Value");

            Console.WriteLine("Now, counter value is {0}", counter);

            counter = remoteServer.Invoke(name, "Add", new object[] { 5 });
            counter = remoteServer.GetAttribute(name, "Value");

            Console.WriteLine("Now, counter value is {0}", counter);

            counter = remoteServer.Invoke(name, "Reset", new object[] { });
            counter = remoteServer.GetAttribute(name, "Value");

            Console.WriteLine("Now, counter value is {0}", counter);            

            Console.ReadKey();            
         }
      }      
   }
}