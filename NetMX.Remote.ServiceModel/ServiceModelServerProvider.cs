using System;

namespace NetMX.Remote.ServiceModel
{
   public sealed class ServiceModelServerProvider : INetMXConnectorServerFactory
   {
      public INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
      {
         return new ServiceModelConnectorServer(serviceUrl, server);
      }
   }
}
