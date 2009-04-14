using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using NetMX;

namespace ConsoleSample
{
   class Program
   {
      static void Main(string[] args)
      {
         IApplicationContext ctx = ContextRegistry.GetContext();
         IMBeanServer localServer = (IMBeanServer) ctx.GetObject("MBeanServer");
         localServer.Invoke("Domain:name=SampleComponent", "Start", new object[] { });
         Console.ReadLine();
      }
   }
}
