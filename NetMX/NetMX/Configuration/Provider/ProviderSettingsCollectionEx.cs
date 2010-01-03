using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Simon.Configuration.Provider;

namespace NetMX.Configuration.Provider
{
   [ConfigurationCollection(typeof(ProviderSettingsEx))]
   public sealed class ProviderSettingsCollectionEx : ConfigurationElementCollection
   {
      // Fields
      private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

      // Methods
      public ProviderSettingsCollectionEx()
         : base(StringComparer.OrdinalIgnoreCase)
      {
      }

      public void Add(ProviderSettingsEx provider)
      {
         if (provider != null)
         {
            this.BaseAdd(provider);
         }
      }

      public void Clear()
      {
         base.BaseClear();
      }

      protected override ConfigurationElement CreateNewElement()
      {
         return new ProviderSettingsEx();
      }

      protected override object GetElementKey(ConfigurationElement element)
      {
         return ((ProviderSettingsEx)element).Name;
      }

      public void Remove(string name)
      {
         base.BaseRemove(name);
      }

      // Properties
      public ProviderSettingsEx this[int index]
      {
         get
         {
            return (ProviderSettingsEx)base.BaseGet(index);
         }
         set
         {
            if (base.BaseGet(index) != null)
            {
               base.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
         }
      }

      public new ProviderSettingsEx this[string key]
      {
         get
         {
            return (ProviderSettingsEx)base.BaseGet(key);
         }
      }

      protected override ConfigurationPropertyCollection Properties
      {
         get
         {
            return _properties;
         }
      }
   }
}