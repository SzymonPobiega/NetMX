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
		public static void InstantiateProviders<T>(ProviderSettingsCollectionEx settingsCollection, IDictionary<string, T> providers)
			where T : ProviderBaseEx
		{
			foreach (ProviderSettingsEx settings in settingsCollection)
			{
				T newProvider = InstantiateProvider<T>(settings);
				providers[newProvider.Name] = newProvider;
			}
		}

		private static T InstantiateProvider<T>(ProviderSettingsEx providerSettings)
			where T : ProviderBaseEx
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
					throw new ArgumentException("Provider must implemenent type " + typeof(T).AssemblyQualifiedName);
				}
				ProviderBaseEx provider = (ProviderBaseEx)Activator.CreateInstance(providerType);
				NameValueCollection parameters = providerSettings.Parameters;
				NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
				foreach (string str2 in parameters)
				{
					config[str2] = parameters[str2];
				}
				provider.Initialize(providerSettings.Name, config, providerSettings.NestedElement);
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
