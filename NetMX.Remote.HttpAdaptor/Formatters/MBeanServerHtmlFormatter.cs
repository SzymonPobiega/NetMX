using System.IO;
using System.Web.Http.Routing;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanServerHtmlFormatter : HtmlFormatterBase
    {
        public override bool CanWriteType(System.Type type)
        {
            return type == typeof(MBeanServerResource);
        }

        protected override void WriteBody(object value, TextWriter writer)
        {
            var typedValue = (MBeanServerResource)value;
            writer.WriteLine("<div>");
            writer.WriteLine("<h1>MBean server {0} (version {1}) powered by <a href=\"http://github.com/SzymonPobiega/NetMX\">NetMX</a></h1>", typedValue.InstanceName, typedValue.Version);
            writer.WriteLine("<h3>Registered MBeans:</h3>");
            writer.WriteLine("<ul id=\"beanTree\" class=\"filetree\">");
            WriteDomainItems(writer, typedValue.RootDomain);
            writer.WriteLine("</ul>");
            writer.WriteLine("<a href=\"{0}\">Dynamic UI</a>", typedValue.DynamicViewHref);
            writer.WriteLine("</div>");

            writer.WriteLine(@"
<script type=""text/javascript"" src=""UI/jquery-1.7.2.min.js""></script>
<script type=""text/javascript"" src=""UI/jquery.treeview.js""></script>
<script type=""text/javascript"">
    $(""#beanTree"").treeview(); 
</script>
");
        }

        private static void WriteDomainItems(TextWriter writer, MBeanDomain domain)
        {
            foreach (var bean in domain.Beans)
            {
                writer.WriteLine("<li><span class=\"file\"><a href=\"{0}\">{1}</a></span></li>", bean.HRef, bean.ShortName);
            }
            foreach (var subdomain in domain.Subdomains)
            {
                writer.WriteLine("<li>");
                WriteDomain(writer, subdomain);
                writer.WriteLine("</li>");
            }
        }

        private static void WriteDomain(TextWriter writer, MBeanDomain domain)
        {
            writer.WriteLine("<span class=\"folder\">{0}</span>", domain.Name);
            writer.WriteLine("<ul>");
            WriteDomainItems(writer, domain);
            writer.WriteLine("</ul>");
        }

        protected override string Title
        {
            get { return "MBean server"; }
        }
    }
}