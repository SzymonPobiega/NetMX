#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Specialized;
#endregion

namespace Simon.Configuration.Provider
{
	public static class ProvidersHelper
	{
		public static void InstantiateProviders<T>(ProviderSettingsCollection settingsCollection, IDictionary<string, T> providers)
			where T : ProviderBase
		{
			foreach (ProviderSettings settings in settingsCollection)
			{
				T newProvider = InstantiateProvider<T>(settings);
				providers[newProvider.Name] = newProvider;
			}
		}

		private static T InstantiateProvider<T>(ProviderSettings providerSettings)
			where T : ProviderBase
		{			
			try
			{
				string providerTypeName = (providerSettings.Type == null) ? null : providerSettings.Type.Trim();
				if (string.IsNullOrEmpty(providerTypeName))
				{
					throw new ArgumentException("Provider type name not specified", "providerSettings");
				}
				Type providerType = Type.GetType(providerTypeName, true);
				if (!typeof(T).IsAssignableFrom(providerType))
				{
					throw new ArgumentException("Provider must implemenent type "+typeof(T).AssemblyQualifiedName);
				}				
				ProviderBase provider = (ProviderBase)Activator.CreateInstance(providerType);
				NameValueCollection parameters = providerSettings.Parameters;
				NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
				foreach (string str2 in parameters)
				{
					config[str2] = parameters[str2];
				}
				provider.Initialize(providerSettings.Name, config);
				return (T)provider;
			}
			catch (Exception exception)
			{
				if (exception is ConfigurationException)
				{
					throw;
				}
				throw new ConfigurationErrorsException(exception.Message, providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber);
			}			
		}

	}
}
