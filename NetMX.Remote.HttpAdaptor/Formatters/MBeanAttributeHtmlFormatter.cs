using System.IO;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanAttributeHtmlFormatter : HtmlFormatterBase
    {
        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }

        protected override void WriteBody(object value, TextWriter writer)
        {
            var typedValue = (MBeanAttributeResource)value;
            writer.WriteLine("<div>");
            writer.WriteLine(string.Format("<div><span>{0}</span><span>{1}</span></div>",typedValue.Name,typedValue.Value));
            writer.WriteLine(string.Format("<a href=\"{0}\">MBean</a>",typedValue.MBeanHRef));
            writer.WriteLine("</div>");
        }

        protected override string Title
        {
            get { return "MBean attribute"; }
        }
    }
}