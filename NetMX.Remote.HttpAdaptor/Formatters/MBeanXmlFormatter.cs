using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanXmlFormatter : XmlMediaTypeFormatter
    {
        private const string ContentType = "application/vnd.netmx.bean+xml";

        public MBeanXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType));
        }

        protected override bool CanReadType(System.Type type)
        {
            return false;
        }

        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanResource);
        }
    }
}