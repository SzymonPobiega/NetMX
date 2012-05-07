using System.IO;
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
        private const string SimpleContentType = "application/vnd.netmx.attr.simple+xml";
        private const string ComplexContentType = "application/vnd.netmx.attr.complex+xml";

        public MBeanAttributeXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SimpleContentType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ComplexContentType));
            Encoding = new UTF8Encoding(false, true);
        }

        protected override bool CanReadType(System.Type type)
        {
            return type != typeof(IKeyValueModel);
        }

        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }

        protected override Task<object> OnReadFromStreamAsync(System.Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        using (var streamReader = new StreamReader(stream, Encoding))
                        {
                            var root = XElement.Load(streamReader);
                            object result = null;
                            if (contentHeaders.ContentType.MediaType == SimpleContentType)
                            {
                                result = new MBeanSimpleValueAttributeResource
                                             {
                                                 Type = root.Element("Type").Value,
                                                 SimpleValue = root.Element("Value").Value
                                             };
                            }
                            return result;
                        }
                    });
        }

        protected override Task OnWriteToStreamAsync(System.Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var typedValue = (MBeanSimpleValueAttributeResource) value;

                        var root = new XElement("MBeanAttribute",
                                                new XElement("Type", typedValue.Type),
                                                new XElement("Value", typedValue.SimpleValue),
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

        
    }
}