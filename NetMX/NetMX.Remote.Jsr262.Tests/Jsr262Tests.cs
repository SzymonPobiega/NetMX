using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetMX.Remote.Jsr262.Tests
{
   [TestFixture]
   public class Jsr262Tests
   {      
      [Test]
      public void TestGetDefaultDomain()
      {
         string defaultDomain = _remoteServer.GetDefaultDomain();

         Assert.AreEqual("NetMXImplementation", defaultDomain);
      }

      [Test]
      public void TestGetDomains()
      {
         IEnumerable<string> domains = _remoteServer.GetDomains();

         Assert.AreEqual(2, domains.Count());
         Assert.IsTrue(domains.Contains("NetMXImplementation"));
         Assert.IsTrue(domains.Contains("Sample"));
      }

      [Test]
      public void TestQueryNamesNoRestrictions()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames(null, null);

         Assert.AreEqual(2, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Sample:a=b")));
         Assert.IsTrue(names.Contains(new ObjectName("NetMXImplementation:type=MBeanServerDelegate")));
      }

      [Test]
      public void TestQueryNamesObjectNameRestriction()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames("Sample:a=b", null);

         Assert.AreEqual(1, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Sample:a=b")));
      }

      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         Sample o = new Sample();
         ObjectName name = new ObjectName("Sample:a=b");
         _server.RegisterMBean(o, name);
         Uri serviceUrl = new Uri("http://localhost:13545/MBeanServer");

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = NetMXConnectorFactory.Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}
