using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace NetMX.Remote.ServiceModel
{
   public sealed class ServiceModelConnectorServer : INetMXConnectorServer
   {
      private readonly IMBeanServer _beanServer;
      private readonly Uri _serviceUrl;
      private readonly ServiceHost _serviceHost;

      public ServiceModelConnectorServer(Uri serviceUrl, IMBeanServer beanServer)
      {
         _beanServer = beanServer;
         _serviceUrl = serviceUrl;
         _serviceHost = new ServiceHost(new MBeanServerService(beanServer), _serviceUrl);         
      }

      public void Dispose()
      {
         Stop();
      }

      public IMBeanServer MBeanServer
      {
         get { return _beanServer; }
      }

      public void Start()
      {        
         _serviceHost.Open();
      }

      public void Stop()
      {
         _serviceHost.Close();
      }
   }
}