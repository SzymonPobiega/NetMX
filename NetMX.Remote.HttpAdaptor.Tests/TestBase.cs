using NUnit.Framework;

namespace NetMX.Remote.HttpAdaptor.Tests
{
    public abstract class TestBase
    {
        protected IMBeanServer Server;
        protected SelfHostingHttpAdaptor Adaptor;

        [SetUp]
        public void Initialize()
        {
            Server = MBeanServerFactory.CreateMBeanServer();

            Server.RegisterMBean(new Sample(), "sample:a=b");

            Adaptor = new SelfHostingHttpAdaptor(Server, "http://localhost:12345/adaptor");
            Adaptor.Start();
        }

        [TearDown]
        public void CleanUp()
        {
            Adaptor.Stop();
        }
    }
}