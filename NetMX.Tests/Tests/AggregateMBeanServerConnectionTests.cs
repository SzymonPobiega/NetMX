using System;
using NetMX.Server;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Tests
{
    [TestFixture]
    public class AggregateMBeanServerConnectionTests
    {
        private Test _bean1;
        private IMBeanServer _server1;
        private Test _bean2;
        private IMBeanServer _server2;
        private Test _bean3;
        private IMBeanServer _server3;
        private AggregateMBeanServerConnection _aggregateConnection;

        [SetUp]
        public void SetUp()
        {
            _bean1 = new Test();
            _server1 = MBeanServerFactory.CreateMBeanServer("1");
            _server1.RegisterMBean(_bean1, "someDomain:t=test");

            _bean2 = new Test();
            _server2 = MBeanServerFactory.CreateMBeanServer("2");
            _server2.RegisterMBean(_bean2, "someDomain:t=test");

            _bean3 = new Test();
            _server3 = MBeanServerFactory.CreateMBeanServer("3");
            _server3.RegisterMBean(_bean3, "someDomain:t=test");
            _server3.RegisterMBean(_bean3, "invalidPrefix.someDomain:t=test");

            _aggregateConnection = new AggregateMBeanServerConnection(_server3);
            _aggregateConnection.AddChildServer("one", _server1);
            _aggregateConnection.AddChildServer("two", _server2);
        }

        [Test]
        public void It_redirects_commands_to_proper_delegate()
        {
            _aggregateConnection.SetAttribute("one.someDomain:t=test","Value",1);
            Assert.AreEqual(1, _bean1.Value);
        }

        [Test]
        public void It_redirects_queries_to_proper_delegate()
        {
            _bean1.Value = 2;
            var value = (int)_aggregateConnection.GetAttribute("one.someDomain:t=test", "Value");
            Assert.AreEqual(2, value);
        }

        [Test]
        public void It_redirects_commands_to_default_server_when_no_delegate_found()
        {
            _aggregateConnection.SetAttribute("invalidPrefix.someDomain:t=test", "Value", 3);
            Assert.AreEqual(3, _bean3.Value);
        }

        [Test]
        public void It_redirects_queries_to_default_server_when_no_delegate_found()
        {
            _bean3.Value = 4;
            var value = (int)_aggregateConnection.GetAttribute("invalidPrefix.someDomain:t=test", "Value");
            Assert.AreEqual(4, value);
        }

        [Test]
        public void It_redirects_commands_to_default_server_when_no_prefix()
        {
            _aggregateConnection.SetAttribute("someDomain:t=test", "Value", 5);
            Assert.AreEqual(5, _bean3.Value);
        }

        [Test]
        public void It_redirects_queries_to_default_server_when_no_prefix()
        {
            _bean3.Value = 6;
            var value = (int)_aggregateConnection.GetAttribute("someDomain:t=test", "Value");
            Assert.AreEqual(6, value);
        }

        [Test]
        public void It_returns_aggregate_count_of_all_servers()
        {
            var count = _aggregateConnection.GetMBeanCount();
            Assert.AreEqual(7, count);
        }

        public interface TestMBean
        {
            int Value { get; set; }
        }

        public class Test : TestMBean
        {
            public int Value { get; set; }
        } 
    } 
}
// ReSharper restore InconsistentNaming
