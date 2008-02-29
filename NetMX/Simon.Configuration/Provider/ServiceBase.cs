using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace Simon.Configuration.Provider
{    
    public abstract class ServiceBase<T>
        where T : ProviderBase
    {
        #region MEMBERS
        private Dictionary<string, T> _providers;
        private T _defaultProvider;
        #endregion

        #region PROTECTED INTERFACE
        protected T Default
        {
            get
            {
                LoadProviders();
                if (_defaultProvider == null)
                {
                    throw new ProviderException("Default provider not specified.");
                }
                return _defaultProvider;
            }
        }
        protected T this[string key]
        {
            get
            {
                T provider;
                LoadProviders();
                if (_providers.TryGetValue(key, out provider))
                {
                    return provider;
                }
                throw new ProviderException("Provider type not supported: " + key);
            }
        }
        #endregion

        #region UTILITY
        private void LoadProviders()
        {
            if (_providers == null)
            {
                lock (this)
                {
                    if (_providers == null)
                    {
                        try
                        {
                            _providers = new Dictionary<string, T>();
                            object[] attributes = this.GetType().GetCustomAttributes(typeof(ConfigurationSectionAttribute), true);
                            if (attributes.Length == 0)
                            {
                                throw new ProviderException("Service configuration section name not specified. Use ConfigurationSection attribute.");                            
                            }
                            ConfigurationSectionAttribute sectionAttr = (ConfigurationSectionAttribute)attributes[0];
                            if (sectionAttr.DefaultProvider)
                            {
                                ProviderConfigurationSectionWithDefault section = TypedConfigurationManager.GetSection<ProviderConfigurationSectionWithDefault>(sectionAttr.Name);
                                ProvidersHelper.InstantiateProviders<T>(section.Providers, _providers);
                                _defaultProvider = _providers[section.DefaultProvider];
                            }
                            else
                            {
                                ProviderConfigurationSection section = TypedConfigurationManager.GetSection<ProviderConfigurationSection>(sectionAttr.Name);
                                ProvidersHelper.InstantiateProviders<T>(section.Providers, _providers);
                            }                            
                        }
                        catch (Exception)
                        {
                            _providers = null;
                            throw;
                        }
                    }
                }
            }
        }
        #endregion
    }    
}
