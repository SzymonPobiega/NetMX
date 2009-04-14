using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NetMX.Remote.ServiceModel;

namespace ConsoleManagementClient
{
   class Program
   {
      static void Main(string[] args)
      {
         ChannelFactory<IMBeanServerContract> factory = new ChannelFactory<IMBeanServerContract>(
            new BasicHttpBinding(), "http://localhost:1010/MBeanServer");
         IMBeanServerContract proxy = factory.CreateChannel();

         proxy.Invoke("Domain:name=SampleComponent", "Start", new object[] { });

         proxy.Invoke("Domain:name=SampleComponent", "IntOperation", new object[] { 7 });

         proxy.Invoke("Domain:name=SampleComponent", "StringAndIntOperation", new object[] { "Ala",  7 });
      }
   }
}
