using System;
using System.Configuration;

namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262ConnectorProvider : NetMXConnectorProvider
   {
      private int _enumerationMaxElements = 1500;

      public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
      {
         return new Jsr262Connector(serviceUrl.ToString(), _enumerationMaxElements);
      }

      public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config, ConfigurationElement nestedElement)
      {
         base.Initialize(name, config, nestedElement);
         string tmp = config["enumerationMaxElements"];
         if (tmp != null)
         {
            int value;
            if (!int.TryParse(tmp, out value))
            {
               throw new ConfigurationErrorsException("Max enumeration elements in one message must be integer value.");
            }
            if (value <= 0)
            {
               throw new ConfigurationErrorsException("Max enumeration elements in one message must have positive value.");
            }
            _enumerationMaxElements = value;
         }
      }
   }
}
