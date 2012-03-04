using System;
using NetMX.Server;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Tests
{
    [TestFixture]
    public class DynamicMBeanProxyTests
    {
        private IMBeanServer _server;
        private Sample _bean;

        [Test]
        public void It_can_invoke_operation()
        {
            var reference = new object();
            dynamic proxy = new DynamicMBeanProxy("sample:id=1", _server);
            var result = proxy.Operation(7, reference);
            Assert.AreSame(reference, result);
        }

        [Test]
        public void It_can_get_attribute()
        {
            dynamic proxy = new DynamicMBeanProxy("sample:id=1", _server);
            var result = proxy.Attribute;
            Assert.AreEqual("Text", result);
        }
        
        [Test]
        public void It_can_set_attribute()
        {
            dynamic proxy = new DynamicMBeanProxy("sample:id=1", _server);
            proxy.Attribute = "New text";
            Assert.AreEqual("New text", _bean.Attribute);
        }


        [SetUp]
        public void Initialize()
        {
            _bean = new Sample
                        {
                            Attribute = "Text"
                        };
            _server = new MBeanServer();
            _server.RegisterMBean(_bean, "sample:id=1");
        }

        public interface SampleMBean
        {
            object Operation(int intParam, object referenceParam);
            string Attribute { get; set; }
        }

        public class Sample : SampleMBean
        {
            public object Operation(int intParam, object referenceParam)
            {
                return referenceParam;
            }

            public string Attribute { get; set; }
        }
    }
}
// ReSharper restore InconsistentNaming
