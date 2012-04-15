using System;

namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262ConnectorServerFactory : INetMXConnectorServerFactory
   {
      public INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
      {
         return new Jsr262ConnectorServer(serviceUrl+"/", server);
      }
   }
}
