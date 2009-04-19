using System;
using System.Security.Principal;
using NetMX;
using NetMX.Remote;

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
               object counter = remoteServer.GetAttribute(name, "Counter");
               Console.WriteLine("Counter value is {0}", counter);
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
