using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX.OpenMBean;
using NUnit.Framework;

namespace NetMX.Remote.Tests
{
   [TestFixture]
   public abstract class OpenDynamicTests
   {            
      [Test]
      public void TestQueryNamesNoRestrictions()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames(null, null);

         Assert.AreEqual(2, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Tests:key=value")));
         Assert.IsTrue(names.Contains(new ObjectName("JMImplementation:type=MBeanServerDelegate")));
      }

      [Test]
      public void TestQueryNamesObjectNameRestriction()
      {
         IEnumerable<ObjectName> names = _remoteServer.QueryNames("Tests:key=value", null);

         Assert.AreEqual(1, names.Count());
         Assert.IsTrue(names.Contains(new ObjectName("Tests:key=value")));
      }

      [Test]
      public void TestGetMBeanInfo()
      {
         MBeanInfo info = _remoteServer.GetMBeanInfo("Tests:key=value");
         MBeanInfo referenceInfo = _server.GetMBeanInfo("Tests:key=value");

         Assert.IsTrue(info.IsOpen());
         Assert.AreEqual(referenceInfo, info);
      }

      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         OpenDynamic o = new OpenDynamic();
         ObjectName name = new ObjectName("Tests:key=value");
         _server.RegisterMBean(o, name);
         Uri serviceUrl = GetUri();

         _connectorServer = NetMXConnectorServerFactory.NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = NetMXConnectorFactory.Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      protected abstract Uri GetUri();

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}