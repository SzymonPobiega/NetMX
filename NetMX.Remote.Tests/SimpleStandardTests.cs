using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetMX.Remote.Tests
{
    [TestFixture]
    public abstract class SimpleStandardTests
    {
        [Test]
        public void TestGetDefaultDomain()
        {
            string defaultDomain = _remoteServer.GetDefaultDomain();

            Assert.AreEqual("JMImplementation", defaultDomain);
        }

        [Test]
        public void TestGetMBeanCount()
        {
            int count = _remoteServer.GetMBeanCount();

            Assert.AreEqual(2, count);
        }

        [Test]
        public void TestGetDomains()
        {
            IEnumerable<string> domains = _remoteServer.GetDomains();

            Assert.AreEqual(2, domains.Count());
            Assert.IsTrue(domains.Contains("JMImplementation"));
            Assert.IsTrue(domains.Contains("Tests"));
        }

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

        private IMBeanServer _server;
        private INetMXConnectorServer _connectorServer;
        private INetMXConnector _connector;
        private IMBeanServerConnection _remoteServer;

        [SetUp]
        public void SetUp()
        {
            _server = MBeanServerFactory.CreateMBeanServer();
            SimpleStandard o = new SimpleStandard();
            ObjectName name = new ObjectName("Tests:key=value");
            _server.RegisterMBean(o, name);
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