using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;
using NetMX.Remote.HttpAdaptor;

namespace HttpAdaptorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = "http://localhost:12345/adaptor";

            var server = MBeanServerFactory.CreateMBeanServer();

            server.RegisterMBean(new Sample(), "sample:a=b");

            var adaptor = new SelfHostingHttpAdaptor(server, address);
            adaptor.Start();

            Console.WriteLine("Http adaptor started at {0}. Press <enter> to exit.", address);
            Console.ReadLine();

            adaptor.Stop();
        }
    }
}
