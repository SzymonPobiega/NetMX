using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanAttributeXmlFormatter: MediaTypeFormatter
    {
        public MBeanAttributeXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.netmx.attr+xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }

        public override bool CanReadType(Type type)
        {
            return true; // type != typeof(IKeyValueModel);
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        using (var streamReader = new StreamReader(readStream, Encoding.UTF8))
                        {
                            var root = XElement.Load(streamReader);
                            object result = null;
                            if (content.Headers.ContentType.MediaType == "application/vnd.netmx.attr+xml")
                            {
                                result = new MBeanAttributeResource
                                             {
                                                 Value = DeserializeValue(root.Element("Value"))
                                             };
                            }
                            return result;
                        }
                    });
        }

        private static object DeserializeValue(XElement root)
        {
            if (root.IsEmpty)
            {
                return null;
            }
            var content = root.FirstNode;
            if (content.NodeType == XmlNodeType.Text)
            {
                return ((XText) content).Value;
            }
            var element = (XElement) content;
            if (element.Name == "Array")
            {
                return DeserializeArrayValue(element, x => x.Value);
            }
            if (element.Name == "Composite")
            {
                return DeserializeCompositeValue(element);
            }
            if (element.Name == "Tabular")
            {
                return DeserializeTabularValue(element);
            }
            throw new NotSupportedException("Not supported value type: " + root);
        }

        private static object DeserializeTabularValue(XElement element)
        {
            return DeserializeArrayValue(element, DeserializeCompositeValue);
        }

        private static CompositeData DeserializeCompositeValue(XElement element)
        {
            var properties = element.Elements().Select(x => new CompositeDataProperty(x.Name.LocalName, x.Value));
            return new CompositeData(properties);
        }

        private static object DeserializeArrayValue<T>(XElement element, Func<XElement, T> valueFunction)
        {
            return element.Elements().Select(valueFunction).ToArray();
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var typedValue = (MBeanAttributeResource) value;

                        var root = new XElement("MBeanAttribute",
                                                new XElement("Value", SerializeValue(typedValue.Value)),
                                                new XElement("Parent", typedValue.MBeanHRef),
                                                new XElement("Name", typedValue.Name));

                        using (var streamWriter = new StreamWriter(writeStream, Encoding.UTF8))
                        using (var xmlWriter = XmlWriter.Create(streamWriter))
                        {
                            root.WriteTo(xmlWriter);
                            xmlWriter.Flush();
                        }
                    });
        }

        private static XNode SerializeValue(object value)
        {
            var stringValue = value as String;
            if (stringValue != null)
            {
                return new XText(stringValue);
            }
            var arrayValue = value as string[];
            if (arrayValue != null)
            {
                return SerializeArrayValue(arrayValue, "Array", "Element");
            }
            var compositeValue = value as CompositeData;
            if (compositeValue != null)
            {
                return new XElement("Composite", SerializeCompositeValue(compositeValue));
            }
            var tabularValue = value as CompositeData[];
            if (tabularValue != null)
            {
                return SerializeArrayValue(tabularValue.Select(SerializeCompositeValue), "Tabular", "Row");
            }
            throw new NotSupportedException("Not supported value type: " + value.GetType().FullName);
        }

        private static object[] SerializeCompositeValue(CompositeData compositeValue)
        {
            return compositeValue.Properties.Select(x => new XElement(x.Name, x.Value)).ToArray();
        }

        private static XElement SerializeArrayValue(IEnumerable<object> elements, string parentName, string childName)
        {
            return new XElement(parentName, elements.Select(x => new XElement(childName, x)));
        }
    }
}