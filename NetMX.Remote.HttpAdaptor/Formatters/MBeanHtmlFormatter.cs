using System.IO;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanHtmlFormatter : HtmlFormatterBase
    {
        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanResource);
        }

        protected override void WriteBody(object value, TextWriter writer)
        {
            var typedValue = (MBeanResource)value;
            writer.WriteLine("<div>");
            writer.WriteLine(string.Format("<div><span>{0}</span><span>{1}</span></div>", typedValue.ClassName, typedValue.Description));
            writer.WriteLine("<ul>");
            foreach (var attributeInfo in typedValue.Attributes)
            {
                writer.WriteLine(string.Format("<li><a href=\"{0}\">{1}</a></li>",attributeInfo.HRef,attributeInfo.Name));
            }
            writer.WriteLine("</ul>");
            writer.WriteLine(string.Format("<a href=\"{0}\">MBean server</a>", typedValue.ServerHRef));
            writer.WriteLine("</div>");
        }

        protected override string Title
        {
            get { return "MBean"; }
        }
    }
}