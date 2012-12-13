using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanJsonFormatter : JsonMediaTypeFormatter
    {
        private const string ContentType = "application/vnd.netmx.bean+json";

        public MBeanJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType));
        }

        public override bool CanReadType(System.Type type)
        {
            return false;
        }

        public override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanResource);
        }
    }
}