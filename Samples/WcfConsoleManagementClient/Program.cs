using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NetMX;
using NetMX.OpenMBean;
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

         MBeanInfo info = proxy.GetMBeanInfo("Sample:name=SampleComponent");
         IOpenMBeanInfo openInfo = (IOpenMBeanInfo)proxy.GetMBeanInfo("Sample:name=SampleComponent,OpenMBeanProxy=true");

         proxy.Invoke("Sample:name=SampleComponent", "Start", new object[] { });

         proxy.Invoke("Sample:name=SampleComponent", "IntOperation", new object[] { 7 });

         proxy.Invoke("Sample:name=SampleComponent", "StringAndIntOperation", new object[] { "Ala", 7 });
      }
   }
}
