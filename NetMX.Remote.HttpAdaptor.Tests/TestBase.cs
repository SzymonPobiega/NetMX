using NetMX.OpenMBean;
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
            var dynamicMBean = new SampleDynamicMBean();
            dynamicMBean.AddRow(1, "Simon");
            dynamicMBean.AddRow(2, "John");
            dynamicMBean.SetComposite(3, "Jane");
            dynamicMBean.SetArray(new[]{1, 2.5m, 4m, 5m});

            Server.RegisterMBean(dynamicMBean, "dynamic:a=b");

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