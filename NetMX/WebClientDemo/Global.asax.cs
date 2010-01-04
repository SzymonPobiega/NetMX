using System;
using System.Collections.Generic;
using System.Linq;
using NetMX;
using NetMX.Proxy;
using NetMX.Relation;
using NetMX.Remote;

namespace WebClientDemo
{
   public class Global : System.Web.HttpApplication
   {
      private static IMBeanServer _beanServer;
      private static INetMXConnectorServer _connectorServer;

      private static void InitializeMBeanServer()
      {
         _beanServer = MBeanServerFactory.CreateMBeanServer();

         _beanServer.RegisterMBean(new RelationService(), RelationService.ObjectName);

         Sample sample1 = new Sample();
         Sample sample2 = new Sample();
         Sample sample3 = new Sample();
         _beanServer.RegisterMBean(sample1, "Sample:type=Sample,id=1");
         _beanServer.RegisterMBean(sample2, "Sample:type=Sample,id=2");
         _beanServer.RegisterMBean(sample3, "Sample:type=Sample,id=3");
         
         RelationServiceMBean relationSerice = NetMXProxyExtensions.NewMBeanProxy<RelationServiceMBean>(_beanServer, RelationService.ObjectName);
         relationSerice.CreateRelationType("Binding", new RoleInfo[] {
            new RoleInfo("Source", typeof(SampleMBean), true, false, 1, 1, "Source"),
            new RoleInfo("Destination", typeof(SampleMBean), true, false, 1, 1, "Destination")});

         relationSerice.CreateRelation("Rel1", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=2"))});

         relationSerice.CreateRelation("Rel2", "Binding", new Role[] {
            new Role("Source", new ObjectName("Sample:type=Sample,id=1")),
            new Role("Destination", new ObjectName("Sample:type=Sample,id=3"))});
         
         Uri serviceUrl = new Uri("net.pipe://localhost/MBeanServer");

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _beanServer);         
         _connectorServer.Start();          
      }

      protected void Application_Start(object sender, EventArgs e)
      {
         InitializeMBeanServer();
      }      

      protected void Application_End(object sender, EventArgs e)
      {
         if (_connectorServer != null)
         {
            _connectorServer.Dispose();
         }
      }
   }
}