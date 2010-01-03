using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace NetMX.Configuration.Provider
{
   public abstract class ProviderBaseEx		
   {
      // Fields
      private string _Description;
      private bool _Initialized;
      private string _name;

      // Methods
      protected ProviderBaseEx()
      {
      }

      public virtual void Initialize(string name, NameValueCollection config, ConfigurationElement nestedElement)
      {
         lock (this)
         {
            if (this._Initialized)
            {
               throw new InvalidOperationException("Provider already initialized.");
            }
            this._Initialized = true;
         }
         if (string.IsNullOrEmpty(name))
         {
            throw new ArgumentNullException("name");
         }			
         this._name = name;
         if (config != null)
         {
            this._Description = config["description"];
            config.Remove("description");
         }
      }

      // Properties
      public virtual string Description
      {
         get
         {
            if (!string.IsNullOrEmpty(this._Description))
            {
               return this._Description;
            }
            return this.Name;
         }
      }

      public virtual string Name
      {
         get
         {
            return this._name;
         }
      }
   }
}