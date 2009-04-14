using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.OpenMBean.Mapper;
using NetMX.Relation;
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

         localServer.RegisterMBean(new RelationService(), RelationService.ObjectName);

         OpenMBeanMapperService mapperService = new OpenMBeanMapperService(new ObjectName[] { "Sample:*" });
         localServer.RegisterMBean(mapperService, ":type=OpenMBeanMapperService");

         localServer.Invoke("Sample:name=SampleComponent,OpenMBeanProxy=true", "Start", new object[] { });
         Console.ReadLine();
      }
   }
}
