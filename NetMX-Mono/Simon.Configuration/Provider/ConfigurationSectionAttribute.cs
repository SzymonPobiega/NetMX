#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace Simon.Configuration.Provider
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        private string _name;
        /// <summary>
        /// Name of associated configuration section.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private bool _withDefault;
        /// <summary>
        /// Does associated configuration section defines default provider?
        /// </summary>
        public bool DefaultProvider
        {
            get { return _withDefault; }
            set { _withDefault = value; }
        }
        public ConfigurationSectionAttribute()
        {
        }
        public ConfigurationSectionAttribute(string name)
        {
            _name = name;
        }
    }
}
