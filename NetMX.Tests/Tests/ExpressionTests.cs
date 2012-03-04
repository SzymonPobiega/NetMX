using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NetMX.Server;
using NUnit.Framework;

namespace NetMX.Tests
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void Can_specify_alternatives_in_query()
        {
            Assert.IsTrue(_server.QueryNames(null, new OrExp(
                                                      new EqualExp(new NumericAttributeExp("IntAttribute"), new ConstantExp<decimal>(1)),
                                                      new EqualObjectExp(new AttributeExp<string>("StringAttribute"), new ConstantExp<string>("AAA"))))
                             .Contains(new ObjectName("sample:id=1")));            
        }

        private IMBeanServer _server;

        private Sample _firstBean;
        private Sample _secondBean;

        [SetUp]
        public void Initialize()
        {
            _firstBean = new Sample()
                            {
                                IntAttribute = 1,
                                LongAttribute = 2,
                                DoubleAttribute = 3,
                                DecimalAttribute = 4,
                                StringAttribute = "5"
                            };
            _secondBean = new Sample()
            {
                IntAttribute = 10,
                LongAttribute = 20,
                DoubleAttribute = 30,
                DecimalAttribute = 40,
                StringAttribute = "50"
            };
            _server = new MBeanServer();
            _server.RegisterMBean(_firstBean, "sample:id=1");
            _server.RegisterMBean(_secondBean, "sample:id=2");
        }

        public interface SampleMBean
        {
            int IntAttribute { get; }
            long LongAttribute { get; }
            double DoubleAttribute { get; }
            decimal DecimalAttribute { get; }
            string StringAttribute { get; }
        }

        public class Sample : SampleMBean
        {
            public int IntAttribute { get; set; }
            public long LongAttribute { get; set; }
            public double DoubleAttribute { get; set; }
            public decimal DecimalAttribute { get; set; }
            public string StringAttribute { get; set; }
        }
    }
}
