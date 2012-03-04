using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET;
using WSMan.NET.Addressing;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262
{
    internal static class ObjectNameSelector
    {
        private const string ObjectName = "ObjectName";

        public static IEnumerable<Selector> CreateSelectorSet(this ObjectName objectName)
        {
            return objectName != null 
                ? new[] { new Selector(ObjectName, objectName) } 
                : new Selector[] { };
        }

        public static ObjectName ExtractObjectName(this EndpointReference address)
        {
            return address.GetProperty<SelectorSetHeader>().ExtractObjectName();
        }

        public static ObjectName ExtractObjectName(this SelectorSetHeader selectors)
        {
            return selectors.Selectors.ExtractObjectName();
        }

        public static EndpointReference CreateEndpointAddress(ObjectName name)
        {
            return new EndpointReference("http://example.com")
                .AddProperty(CreateSelectorSetHeader(name), false);
        }

        public static ObjectName ExtractObjectName(this IEnumerable<Selector> selectors)
        {
            return selectors
                .Where(selector => selector.Name == ObjectName)
                .Select(selector => selector.SimpleValue)
                .FirstOrDefault();
        }

        public static SelectorSetHeader CreateSelectorSetHeader(this ObjectName name)
        {
            return new SelectorSetHeader(CreateSelectorSet(name));
        }
    }
}
