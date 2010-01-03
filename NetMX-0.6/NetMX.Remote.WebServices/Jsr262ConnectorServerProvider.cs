using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMX.Remote.WebServices
{
   public sealed class Jsr262ConnectorServerProvider : NetMXConnectorServerProvider
   {
      public override INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
      {
         return new Jsr262ConnectorServer(serviceUrl, server);
      }
   }
}
