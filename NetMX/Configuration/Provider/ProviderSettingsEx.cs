using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;

namespace NetMX.Configuration.Provider
{
   public sealed class ProviderSettingsEx : ConfigurationElement
   {
      // Fields
      private ConfigurationPropertyCollection _properties;
      private NameValueCollection _PropertyNameCollection;
      private readonly ConfigurationProperty _propName;
      private readonly ConfigurationProperty _propType;
      private readonly ConfigurationProperty _propNestedTypeName;
      private ConfigurationProperty _propNestedType;

      // Methods
      public ProviderSettingsEx()
      {
         this._propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
         this._propType = new ConfigurationProperty("type", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
         this._propNestedTypeName = new ConfigurationProperty("nestedType", typeof(string), "", ConfigurationPropertyOptions.None);
         this._properties = new ConfigurationPropertyCollection();
         this._properties.Add(this._propName);
         this._properties.Add(this._propType);
         this._properties.Add(this._propNestedTypeName);
         this._PropertyNameCollection = null;
      }

      public ProviderSettingsEx(string name, string type)
         : this()
      {
         this.Name = name;
         this.Type = type;
      }
		
      protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
      {
         ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), value);
         this._properties.Add(property);
         base[property] = value;
         this.Parameters[name] = value;
         return true;
      }

      protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
      {			
         Type nestedType = System.Type.GetType(this.NestedTypeName, true);
         _propNestedType = new ConfigurationProperty(elementName, nestedType, null);
         INestedConfigurationElement elem = (INestedConfigurationElement) Activator.CreateInstance(nestedType);
         elem.Init();
         elem.Deserialize(reader);
         base[_propNestedType] = (ConfigurationElement)elem;
         return true;
      }		

      // Properties
      [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
      public string Name
      {
         get
         {
            return (string)base[this._propName];
         }
         set
         {
            base[this._propName] = value;
         }
      }

      [ConfigurationProperty("nestedType", IsRequired = false, IsKey = false)]
      public string NestedTypeName
      {
         get
         {
            return (string)base[this._propNestedTypeName];
         }
         set
         {
            base[this._propNestedTypeName] = value;
         }
      }

      [ConfigurationProperty("nestedValue", IsRequired = false, IsKey = false)]
      public ConfigurationElement NestedElement
      {
         get
         {
            if (_propNestedType != null)
            {
               return (ConfigurationElement)base[this._propNestedType];
            }
            return null;
         }			
      }

      public NameValueCollection Parameters
      {
         get
         {
            if (this._PropertyNameCollection == null)
            {
               lock (this)
               {
                  if (this._PropertyNameCollection == null)
                  {
                     this._PropertyNameCollection = new NameValueCollection(StringComparer.Ordinal);
                     foreach (object obj2 in this._properties)
                     {
                        ConfigurationProperty property = (ConfigurationProperty)obj2;
                        if ((property.Name != "name") && (property.Name != "type") && (property.Name != "nestedType"))
                        {
                           this._PropertyNameCollection.Add(property.Name, (string)base[property]);
                        }
                     }
                  }
               }
            }
            return this._PropertyNameCollection;
         }
      }

      protected override ConfigurationPropertyCollection Properties
      {
         get
         {
            //this.UpdatePropertyCollection();
            return this._properties;
         }
      }

      [ConfigurationProperty("type", IsRequired = true)]
      public string Type
      {
         get
         {
            return (string)base[this._propType];
         }
         set
         {
            base[this._propType] = value;
         }
      }
   }
}