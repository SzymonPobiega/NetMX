#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace NetMX.Configuration.Provider
{
   public class ProviderConfigurationSectionWithDefault : ProviderConfigurationSection
   {		
      [ConfigurationProperty("defaultProvider", IsRequired = true)]
      public string DefaultProvider
      {
         get { return (string)this["defaultProvider"]; }
      }
   }
}