using System;

namespace NetMX.Remote.ServiceModel
{
   public sealed class ServiceModelConnectorProvider : NetMXConnectorProvider
   {
      private const string _configurationNameProperty = "endpointName";
      private string _configurationName;

      public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config, System.Configuration.ConfigurationElement nestedElement)
      {
         base.Initialize(name, config, nestedElement);
         _configurationName = config[_configurationNameProperty];
      }
      public override INetMXConnector NewNetMXConnector(Uri serviceUrl)
      {
         return new ServiceModelConnector(_configurationName, serviceUrl);
      }
   }
}
