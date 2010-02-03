using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class QueryNamesTests
   {
      private static readonly Uri _serviceUrl = new Uri("http://localhost:13545/MBeanServer");
      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      
      [Test]
      public void Large_result_sets_can_be_returned_using_multiple_pull_requests()
      {
         using (INetMXConnector connector = new Jsr262Connector(_serviceUrl, null, 100))
         {
            connector.Connect(null);
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

            Assert.AreEqual(1001, remoteServer.QueryNames(null, null).Count());
         }
      }

      [Test]
      public void Large_result_sets_can_be_returned_using_specific_binding_configuration()
      {
         using (INetMXConnector connector = new Jsr262Connector(_serviceUrl, "LargeMessages", 1500))
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

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(_serviceUrl, _server);
         _connectorServer.Start();         
      }

      [TearDown]
      public void TearDown()
      {         
         _connectorServer.Dispose();
      }
   }
}