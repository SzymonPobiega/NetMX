using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;
using NetMX.Relation;
using NetMX.Remote;
using NetMX.Server.GenericMBeans;

namespace CLRMonitoringDemo
{
   class Program
   {
      static void Main(string[] args)
      {
         //Server side
         IMBeanServer server = MBeanServerFactory.CreateMBeanServer("PlatformMBeanServer");         
         PerfCounterMBean processMBean = new PerfCounterMBean("Process", true, new[] {"% Processor Time"});
         server.RegisterMBean(processMBean, "CLR:type=Process");
         
         //Client side
         Console.WriteLine("Attributes of 'CLR:type=Process' MBean:");
         foreach (AttributeValue v in server.GetAttributes("CLR:type=Process", server.GetMBeanInfo("CLR:type=Process").Attributes.Select(x => x.Name).ToArray()))
         {
            Console.WriteLine("{0}: {1}", v.Name, v.Value);
         }
         Console.WriteLine();
         Console.WriteLine("Attributes of 'CLR:type=Memory' MBean:");
         foreach (AttributeValue v in server.GetAttributes("CLR:type=Memory", server.GetMBeanInfo("CLR:type=Memory").Attributes.Select(x => x.Name).ToArray()))
         {
            Console.WriteLine("{0}: {1}", v.Name, v.Value);
         }
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();         
      }
   }
}