using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NetMX.Remote.HttpAdaptor.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanAttributeJsonFormatter: MediaTypeFormatter
    {
        public MBeanAttributeJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.netmx.attr.simple+json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.netmx.attr.complex+json"));
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
                            using (var jsonTextReader = new JsonTextReader(streamReader))
                            {
                                var deserialized = JObject.Load(jsonTextReader);
                                return (object) new MBeanSimpleValueAttributeResource
                                                    {
                                                        Type = deserialized["type"].Value<string>(),
                                                        SimpleValue = deserialized["value"].Value<string>()
                                                    };
                            }
                        }
                    });
        }

        protected override Task OnWriteToStreamAsync(System.Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var jsonObject = new JObject();
                        var typedValue = (MBeanSimpleValueAttributeResource) value;
                        jsonObject["type"] = typedValue.Type;
                        jsonObject["value"] = typedValue.SimpleValue;
                        jsonObject["parent"] = typedValue.MBeanHRef;
                        jsonObject["name"] = typedValue.Name;
                        using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(stream, Encoding)) {CloseOutput = false})
                        {
                            jsonObject.WriteTo(jsonTextWriter);
                            jsonTextWriter.Flush();
                        }
                    });
        }

        
    }
}