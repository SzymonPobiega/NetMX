using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.Relation;
using NetMX.Remote;
using NetMX.Default.GenericMBeans;

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
         object value = server.GetAttribute("CLR:type=Process", "% Processor Time");
         Console.WriteLine("% Processor Time: {0}", value);
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();         
      }
   }
}