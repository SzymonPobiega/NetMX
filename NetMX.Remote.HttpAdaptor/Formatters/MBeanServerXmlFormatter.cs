using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanServerXmlFormatter : XmlMediaTypeFormatter
    {
        private const string ContentType = "application/vnd.netmx.server+xml";

        public MBeanServerXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType));
        }

        public override bool CanReadType(System.Type type)
        {
            return false;
        }

        public override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanServerResource);
        }
    }
}