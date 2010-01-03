using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Relation;
using NetMX.Remote;
using NetMX.Default.GenericMBeans;

namespace CLRMonitoringSample
{
   class Program
   {
      static void Main(string[] args)
      {
         IMBeanServer server = MBeanServerFactory.CreateMBeanServer("PlatformMBeanServer");         
         PerfCounterMBean processMBean = new PerfCounterMBean("Process", true, new[] {"% Processor Time"});
         server.RegisterMBean(processMBean, "CLR:type=Process");
         
         Uri serviceUrl = new Uri("tcp://localhost:1234/MBeanServer.tcp");

         using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
         {
            connectorServer.Start();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
         }
      }
   }
}
