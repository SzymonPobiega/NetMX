using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NetMX.Remote.Jsr262
{
   public sealed class Jsr262ConnectorProvider : NetMXConnectorProvider
   {
      private int _enumerationMaxElements = 1500;
      private string _bindingConfiguration;

      public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
      {
         return new Jsr262Connector(serviceUrl, _bindingConfiguration, _enumerationMaxElements);
      }

      public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config, System.Configuration.ConfigurationElement nestedElement)
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
         _bindingConfiguration = config["bindingConfiguration"];
      }
   }
}
