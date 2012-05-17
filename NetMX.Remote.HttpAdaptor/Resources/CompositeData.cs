using System.Collections.Generic;
using System.Linq;

namespace NetMX.Remote.HttpAdaptor.Resources
{    
    public class CompositeData
    {
        private readonly List<CompositeDataProperty> _properties;

        public CompositeData(IEnumerable<CompositeDataProperty> properties)
        {
            _properties = properties.ToList();
        }

        public IEnumerable<CompositeDataProperty> Properties
        {
            get { return _properties; }
        }
    }

    public class CompositeDataProperty
    {
        private readonly string _name;
        private readonly string _value;

        public CompositeDataProperty(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }
    }
}