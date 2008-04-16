using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Reflection;

namespace Simon.Configuration.Provider
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

		private string GetProperty(string PropName)
		{
			if (this._properties.Contains(PropName))
			{
				ConfigurationProperty property = this._properties[PropName];
				if (property != null)
				{
					return (string)base[property];
				}
			}
			return null;
		}

		//protected override bool IsModified()
		//{
		//   if (!this.UpdatePropertyCollection())
		//   {
		//      return base.IsModified();
		//   }
		//   return true;
		//}

		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), value);
         AddProperty(property);
			base[property] = value;
			 this.Parameters[name] = value;
			return true;
		}
      
      private void AddProperty(ConfigurationProperty property)
      {
         /* to jest hack, przy dodawaniu nowego property do Properties nie jest odswierzana informacja o propertisach w ElementInformation.Properties 
          * wiec trzeba recznie najpierw dodac nasze property ta metoda do ElementInformation.Properties a dopiero pozniej 
          * probowac sie do niej dostac przez odwolania w stylu base[property] = value; lub	 this.Parameters[name] = value;
          *  ElementInformation.Properties wypelniane jest wylacznie w konstruktorze, ten kod poprzez refleksje ze wzgledu na metody interal dodaje do kolekcji ElementInformation.Properties nowe property ktore nie jest zdefiniowane stricte na tej klasie jak np. name, type
			 */
         this._properties.Add(property);
         // pobranie konstruktora PropertyInformation
         ConstructorInfo ctor = typeof(PropertyInformation).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]{typeof(ConfigurationElement), typeof(ConfigurationProperty)}, null);
         // stworzenie nowej instancji obiektu PropertyInformation  
         PropertyInformation propertyInformation =ctor.Invoke(new object[]{this, property}) as PropertyInformation;
         // pobranie metody dodajacej nowe elementy na kolekcji propertiesow ElementInformation.Properties
		   MethodInfo mi = ElementInformation.Properties.GetType().GetMethod("Add", BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,null,new Type[]{typeof(PropertyInformation)}, null);
         // dodanie property do ElementInformation.Properties
         mi.Invoke(ElementInformation.Properties, new object[]{propertyInformation});

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

		//protected override void Reset(ConfigurationElement parentElement)
		//{
		//   ProviderSettingsEx settings = parentElement as ProviderSettingsEx;
		//   if (settings != null)
		//   {
		//      settings.UpdatePropertyCollection();
		//   }
		//   base.Reset(parentElement);
		//}

		private bool SetProperty(string PropName, string value)
		{
			ConfigurationProperty property;
			if (this._properties.Contains(PropName))
			{
				property = this._properties[PropName];
			}
			else
			{
				property = new ConfigurationProperty(PropName, typeof(string), null);
				this._properties.Add(property);
			}
			base[property] = value;
			return true;
		}

		//protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		//{
		//   ProviderSettingsEx settings = parentElement as ProviderSettingsEx;
		//   if (settings != null)
		//   {
		//      settings.UpdatePropertyCollection();
		//   }
		//   ProviderSettingsEx settings2 = sourceElement as ProviderSettingsEx;
		//   if (settings2 != null)
		//   {
		//      settings2.UpdatePropertyCollection();
		//   }
		//   base.Unmerge(sourceElement, parentElement, saveMode);
		//   this.UpdatePropertyCollection();
		//}

		//internal bool UpdatePropertyCollection()
		//{
		//   bool flag = false;
		//   ArrayList list = null;
		//   if (this._PropertyNameCollection != null)
		//   {
		//      foreach (ConfigurationProperty property in this._properties)
		//      {
		//         if (((property.Name == "name") || (property.Name == "type") || (property.Name == "nestedType")) || (this._PropertyNameCollection.Get(property.Name) != null))
		//         {
		//            continue;
		//         }
		//         if (list == null)
		//         {
		//            list = new ArrayList();
		//         }
		//         if ((base.Values.GetConfigValue(property.Name).ValueFlags & ConfigurationValueFlags.Locked) == ConfigurationValueFlags.Default)
		//         {
		//            list.Add(property.Name);
		//            flag = true;
		//         }
		//      }
		//      if (list != null)
		//      {
		//         foreach (string str in list)
		//         {
		//            this._properties.Remove(str);
		//         }
		//      }
		//      foreach (string str2 in this._PropertyNameCollection)
		//      {
		//         string str3 = this._PropertyNameCollection[str2];
		//         string str4 = this.GetProperty(str2);
		//         if ((str4 == null) || (str3 != str4))
		//         {
		//            this.SetProperty(str2, str3);
		//            flag = true;
		//         }
		//      }
		//   }
		//   this._PropertyNameCollection = null;
		//   return flag;
		//}

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
