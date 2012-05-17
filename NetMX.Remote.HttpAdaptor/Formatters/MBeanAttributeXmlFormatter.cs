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
            Encoding = new UTF8Encoding(false, true);
        }

        protected override bool CanReadType(Type type)
        {
            return type != typeof(IKeyValueModel);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }

        protected override Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        using (var streamReader = new StreamReader(stream, Encoding))
                        {
                            var root = XElement.Load(streamReader);
                            object result = null;
                            if (contentHeaders.ContentType.MediaType == "application/vnd.netmx.attr+xml")
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

        private static Dictionary<string, string> DeserializeCompositeValue(XElement element)
        {
            return element.Elements().ToDictionary(x => x.Name.LocalName, x => x.Value);
        }

        private static object DeserializeArrayValue<T>(XElement element, Func<XElement, T> valueFunction)
        {
            return element.Elements().Select(valueFunction).ToArray();
        }

        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var typedValue = (MBeanAttributeResource) value;

                        var root = new XElement("MBeanAttribute",
                                                new XElement("Value", SerializeValue(typedValue.Value)),
                                                new XElement("Parent", typedValue.MBeanHRef),
                                                new XElement("Name", typedValue.Name));

                        using (var streamWriter = new StreamWriter(stream, Encoding))
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
            var compositeValue = value as Dictionary<string, string>;
            if (compositeValue != null)
            {
                return new XElement("Composite", SerializeCompositeValue(compositeValue));
            }
            var tabularValue = value as Dictionary<string, string>[];
            if (tabularValue != null)
            {
                return SerializeArrayValue(tabularValue.Select(SerializeCompositeValue), "Tabular", "Row");
            }
            throw new NotSupportedException("Not supported value type: " + value.GetType().FullName);
        }

        private static object[] SerializeCompositeValue(Dictionary<string, string> compositeValue)
        {
            return compositeValue.Select(x => new XElement(x.Key, x.Value)).ToArray();
        }

        private static XElement SerializeArrayValue(IEnumerable<object> elements, string parentName, string childName)
        {
            return new XElement(parentName, elements.Select(x => new XElement(childName, x)));
        }
    }
}