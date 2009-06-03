using System;
using System.Collections.Generic;
using System.Security.Principal;
using NetMX;
using NetMX.Remote;
using System.Linq;

namespace WebServicesSample
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
         Uri serviceUrl = new Uri("http://localhost:1010/MBeanServer");

         using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
         {
            connectorServer.Start();

            using (INetMXConnector connector = NetMXConnectorFactory.Connect(serviceUrl, null))
            {
               IMBeanServerConnection remoteServer = connector.MBeanServerConnection;
               MBeanInfo metadata = remoteServer.GetMBeanInfo(name);
               object counter = remoteServer.GetAttribute(name, "Counter");
               Console.WriteLine("Counter value is {0}", counter);
               remoteServer.SetAttribute(name, "Counter", 1);
               counter = remoteServer.GetAttribute(name, "Counter");
               Console.WriteLine("Counter value is {0}", counter);
               int beanCount = remoteServer.GetMBeanCount();
               Console.WriteLine("MBean count is {0}", beanCount);
               string defaultDomain = remoteServer.GetDefaultDomain();
               Console.WriteLine("Default domain is {0}", defaultDomain);
               string domains = string.Join(", ", remoteServer.GetDomains().ToArray());
               Console.WriteLine("Registered domains: {0}", domains);
               Console.WriteLine("Is {0} instance of {1}: {2}", name, typeof(SampleMBean).FullName, remoteServer.IsInstanceOf(name,typeof(SampleMBean).AssemblyQualifiedName));
               Console.WriteLine("Is {0} registered: {1}", name, remoteServer.IsRegistered(name));
               string beans = string.Join(", ", remoteServer.QueryNames(null, null).Select(x => x.ToString()).ToArray());
               Console.WriteLine("Registered MBeans: {0}", beans);
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
