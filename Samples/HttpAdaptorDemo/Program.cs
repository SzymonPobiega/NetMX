using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;
using NetMX.Relation;
using NetMX.Remote.HttpAdaptor;

namespace HttpAdaptorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = "http://localhost:12345/adaptor";

            var server = MBeanServerFactory.CreateMBeanServer("HttpAdaptorDemo");
            server.RegisterMBean(new RelationService(), RelationService.ObjectName);

            server.RegisterMBean(new Sample(), "sample:t=static");

            var dynamicMBean = new SampleDynamicMBean();
            dynamicMBean.AddRow(1, "Simon");
            dynamicMBean.AddRow(2, "John");
            dynamicMBean.SetComposite(3, "Jane");
            dynamicMBean.SetArray(new[] { 1, 2.5m, 4.3m, 5.64m });

            server.RegisterMBean(dynamicMBean, "sample:t=dynamic");

            var relationSerice = server.CreateDynamicProxy(RelationService.ObjectName);
            relationSerice.CreateRelationType("Binding",
                                              new[]
                                                  {
                                                      new RoleInfo("Source", typeof (SampleMBean), true, false, 1, 1,
                                                                   "Source"),
                                                      new RoleInfo("Destination", typeof (SampleDynamicMBean), true, false, 1,
                                                                   1, "Destination")
                                                  });

            relationSerice.CreateRelation("Rel1", "Binding",
                                          new[]
                                              {
                                                  new Role("Source", new ObjectName("sample:t=static")),
                                                  new Role("Destination", new ObjectName("sample:t=dynamic"))
                                              });

            var adaptor = new SelfHostingHttpAdaptor(server, address);
            adaptor.Start();

            Console.WriteLine("Http adaptor started at {0}. Press <enter> to exit.", address);
            Console.ReadLine();

            adaptor.Stop();
        }
    }
}
