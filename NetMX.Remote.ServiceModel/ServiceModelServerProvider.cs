using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;

namespace NetMX.Remote.ServiceModel
{
   public sealed class ServiceModelServerProvider : NetMXConnectorServerProvider
   {
      public override INetMXConnectorServer NewNetMXConnectorServer(Uri serviceUrl, IMBeanServer server)
      {
         return new ServiceModelConnectorServer(serviceUrl, server);
      }
   }
}
