using System;
using System.Linq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class QueryNamesTests
   {      
      [Test]
      public void Quoted_object_name_can_be_returned()
      {
         ObjectName firstQuotedName =
            @"com.acme:name=""AgentConnectorDemo"",type=""log4j"",logger=root,appender=ConsoleAppender";
         ObjectName secondQuotedName =
            @"com.acme:applicationType=""AgentConnectorDemo"",name=""TransportHandler"",type=""SoftwareVersion""";
         _server.RegisterMBean(new Sample(), firstQuotedName);
         _server.RegisterMBean(new Sample(), secondQuotedName);

         ObjectName[] names;
         using (INetMXConnector connector = new Jsr262Connector(_serviceUrl, 100))
         {
            connector.Connect(null);
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;
            names = remoteServer.QueryNames("com.acme:*", null).ToArray();
         }

         Assert.IsTrue(names.Contains(firstQuotedName));
         Assert.IsTrue(names.Contains(secondQuotedName));
      }

      [Test]
      public void Large_result_sets_can_be_returned_using_multiple_pull_requests_set_in_code()
      {
         RegisterBeansForLargeResultSetTests();
         using (INetMXConnector connector = new Jsr262Connector(_serviceUrl, 100))
         {
            connector.Connect(null);
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

            Assert.AreEqual(1001, remoteServer.QueryNames(null, null).Count());
         }
      }

      [Test]
      public void Large_result_sets_can_be_returned_using_multiple_pull_requests_set_in_configuration()
      {
         RegisterBeansForLargeResultSetTests();
         using (INetMXConnector connector = new Jsr262ConnectorFactory().Connect(new Uri(_serviceUrl), null))
         {
            IMBeanServerConnection remoteServer = connector.MBeanServerConnection;

            Assert.AreEqual(1001, remoteServer.QueryNames(null, null).Count());
         }
      } 

      private void RegisterBeansForLargeResultSetTests()
      {
         for (int i = 0; i < 1000; i++)
         {
            Sample o = new Sample();
            ObjectName name = new ObjectName(string.Format("Sample:number={0}", i));
            _server.RegisterMBean(o, name);
         }         
      }

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();

         _connectorServer = new Jsr262ConnectorServerFactory().NewNetMXConnectorServer(new Uri(_serviceUrl), _server);
         _connectorServer.Start();         
      }

      [TearDown]
      public void TearDown()
      {         
         _connectorServer.Dispose();
      }

      private const string _serviceUrl = @"http://localhost:13545/MBeanServer";
      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      
   }
}
// ReSharper restore InconsistentNaming
