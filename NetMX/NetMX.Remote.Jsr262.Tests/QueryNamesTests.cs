using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class QueryNamesTests
   {
      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      
      [Test]
      public void Large_result_sets_can_be_returned_using_multiple_pull_requests()
      {
         using (INetMXConnector connector = new Jsr262Connector(new Uri("http://localhost/MBeanServer"), null, 100))
         {
            connector.Connect(null);
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

            Assert.AreEqual(1001, remoteServer.QueryNames(null, null).Count());
         }
      }

      [Test]
      public void Large_result_sets_can_be_returned_using_specific_binding_configuration()
      {
         using (INetMXConnector connector = new Jsr262Connector(new Uri("http://localhost/MBeanServer"), "LargeMessages", 1500))
         {
            connector.Connect(null);
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

            Assert.AreEqual(1001, remoteServer.QueryNames(null, null).Count());
         }
      }

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         for (int i = 0; i < 1000; i++)
         {
            Sample o = new Sample();
            ObjectName name = new ObjectName(string.Format("Sample:number={0}",i));
            _server.RegisterMBean(o, name);
         }
         Uri serviceUrl = new Uri("http://localhost/MBeanServer");

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();         
      }

      [TearDown]
      public void TearDown()
      {         
         _connectorServer.Dispose();
      }
   }
}