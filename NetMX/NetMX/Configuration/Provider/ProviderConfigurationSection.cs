#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace NetMX.Configuration.Provider
{
   public class ProviderConfigurationSection : ConfigurationSection
   {
      [ConfigurationProperty("providers", IsRequired = true)]
      public ProviderSettingsCollectionEx Providers
      {
         get { return (ProviderSettingsCollectionEx)this["providers"]; }
      }		
   }
}