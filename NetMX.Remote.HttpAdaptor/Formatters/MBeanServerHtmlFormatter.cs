using System.IO;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanServerHtmlFormatter : HtmlFormatterBase
    {
        protected override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanServerResource);
        }

        protected override void WriteBody(object value, TextWriter writer)
        {
            var typedValue = (MBeanServerResource)value;
            writer.WriteLine("<div>");
            writer.WriteLine("<h1>MBean server {0} (version {1}) powered by <a href=\"http://github.com/SzymonPobiega/NetMX\">NetMX</a></h1>", typedValue.InstanceName, typedValue.Version);
            writer.WriteLine("<h3>Registered MBeans:</h3>");
            writer.WriteLine("<ul>");
            foreach (var attributeInfo in typedValue.Beans)
            {
                writer.WriteLine(string.Format("<li><a href=\"{0}\">{1}</a></li>", attributeInfo.HRef, attributeInfo.ObjectName));
            }
            writer.WriteLine("</ul>");
            writer.WriteLine("<a href=\"ui/mbeanserver.htm\">Dynamic UI</a>");
            writer.WriteLine("</div>");
        }

        protected override string Title
        {
            get { return "MBean server"; }
        }
    }
}