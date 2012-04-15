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

      [Test]
      public void TestGetSimpleAttribute()
      {
         _bean.AddRow(1, "First row");
         _bean.AddRow(2, "Second row");

         object referenceValue = _server.GetAttribute("Tests:key=value", "Attribute");
         object value = _remoteServer.GetAttribute("Tests:key=value", "Attribute");
         Assert.AreEqual(referenceValue, value);
      }

      [Test]
      public void TestGetNestedAttribute()
      {
         _bean.AddNestedRow(1, 1, "First row");
         _bean.AddNestedRow(2, 2, "Second row");

         object referenceValue = _server.GetAttribute("Tests:key=value", "NestedTableAttribute");
         object value = _remoteServer.GetAttribute("Tests:key=value", "NestedTableAttribute");
         Assert.AreEqual(referenceValue, value);
      }

      private OpenDynamic _bean;
      private IMBeanServer _server;
      private INetMXConnectorServer _connectorServer;
      private INetMXConnector _connector;
      private IMBeanServerConnection _remoteServer;

      [SetUp]
      public void SetUp()
      {
         _server = MBeanServerFactory.CreateMBeanServer();
         _bean = new OpenDynamic();
         ObjectName name = new ObjectName("Tests:key=value");
         _server.RegisterMBean(_bean, name);
         Uri serviceUrl = GetUri();

         _connectorServer = GetConnectorServerFactory().NewNetMXConnectorServer(serviceUrl, _server);
         _connectorServer.Start();
         _connector = GetConnectorFactory().Connect(serviceUrl, null);
         _remoteServer = _connector.MBeanServerConnection;
      }

      protected abstract Uri GetUri();
      protected abstract INetMXConnectorServerFactory GetConnectorServerFactory();
      protected abstract INetMXConnectorFactory GetConnectorFactory();

      [TearDown]
      public void TearDown()
      {
         _connector.Dispose();
         _connectorServer.Dispose();
      }
   }
}