#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
#endregion

namespace Simon.Configuration.Provider
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
