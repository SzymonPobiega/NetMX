using System;
using System.Collections.Generic;
using System.Text;
using NetMX;
using NetMX.OpenMBean.Mapper;
using NetMX.Remote;
using NetMX.Relation;
using NetMX.OpenMBean;
using NetMX.Proxy;

namespace RemotingServerDemo
{
   class Program
   {
      static void Main(string[] args)
      {
         IMBeanServer server = MBeanServerFactory.CreateMBeanServer();
         
         server.RegisterMBean(new RelationService(), RelationService.ObjectName);
         
         Sample sample1 = new Sample();
         Sample sample2 = new Sample();
         Sample sample3 = new Sample();
         server.RegisterMBean(sample1, "Sample:type=Sample,id=1");
         server.RegisterMBean(sample2, "Sample:type=Sample,id=2");
         server.RegisterMBean(sample3, "Sample:type=Sample,id=3");
         SampleDynamicMBean dynSample = new SampleDynamicMBean();
         dynSample.AddRow(1, "First row");
         dynSample.AddRow(2, "Second row");

         dynSample.AddNestedRow(1, 3, "First nested row");
         dynSample.AddNestedRow(2, 4, "Second nested row");
         server.RegisterMBean(dynSample, "Sample:type=SampleDynamicMBean");

         RelationServiceMBean relationSerice = NetMXProxyExtensions.NewMBeanProxy<RelationServiceMBean>(server, RelationService.ObjectName);
         relationSerice.CreateRelationType("Binding", new RoleInfo[] {
            new RoleInfo("Source", typeof(SampleMBean), true, false, 1, 1, "Source"),
            new RoleInfo("Destination", typeof(SampleMBean), true, false, 1, 1, "Destination")});

         relationSerice.CreateRelation("Rel1", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=2"))});

         relationSerice.CreateRelation("Rel2", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=3"))});

         OpenMBeanMapperService mapperService = new OpenMBeanMapperService(new ObjectName[] {"Sample:type=SampleMappedMBean"});
         server.RegisterMBean(mapperService,":type=OpenMBeanMapperService");

         SampleMapped mappedMBean = new SampleMapped();
         CollectionElement firstColEl = new CollectionElement();
         firstColEl.Elements.Add(new NestedCollectionElement(1, "Name1"));
         firstColEl.Elements.Add(new NestedCollectionElement(2, "Name2"));
         CollectionElement secondColEl = new CollectionElement();
         secondColEl.Elements.Add(new NestedCollectionElement(3, "Name3"));
         secondColEl.Elements.Add(new NestedCollectionElement(4, "Name4"));
         mappedMBean.Add(firstColEl);
         mappedMBean.Add(secondColEl);
         server.RegisterMBean(mappedMBean, "Sample:type=SampleMappedMBean");


         //Uri serviceUrl = new Uri("tcp://localhost:1234/MBeanServer.tcp");
         //Uri serviceUrl = new Uri("http://localhost:1010/MBeanServer");
         Uri serviceUrl = new Uri("net.pipe://localhost/MBeanServer");

         using (INetMXConnectorServer connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, server))
         {
            connectorServer.Start();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
         }
      }
   }
}
