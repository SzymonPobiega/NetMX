using System.Web.Http;
using System.Web.Http.Dispatcher;
using NetMX.Remote.HttpAdaptor.Formatters;

namespace NetMX.Remote.HttpAdaptor
{
    public class HttpAdaptor
    {
        protected void Configure(HttpConfiguration configuration, IMBeanServerConnection serverConnection, string baseUrl)
        {
            configuration.Services.Replace(typeof(IHttpControllerActivator), new ControllerActivator(serverConnection, baseUrl));
            configuration.Formatters.Clear();
            configuration.Formatters.Add(new MBeanAttributeJsonFormatter());
            configuration.Formatters.Add(new MBeanAttributeXmlFormatter());
            configuration.Formatters.Add(new MBeanAttributeHtmlFormatter());

            configuration.Formatters.Add(new MBeanJsonFormatter());
            configuration.Formatters.Add(new MBeanXmlFormatter());
            configuration.Formatters.Add(new MBeanHtmlFormatter());

            configuration.Formatters.Add(new MBeanServerJsonFormatter());
            configuration.Formatters.Add(new MBeanServerXmlFormatter());
            configuration.Formatters.Add(new MBeanServerHtmlFormatter());

            configuration.Routes.MapHttpRoute("server2",
                                              "server",
                                              new { controller = "MBeanServer" });

            configuration.Routes.MapHttpRoute("ui",
                                              "ui/{contentFile}",
                                              new { controller = "UI" });

            configuration.Routes.MapHttpRoute("ui-images",
                                              "ui/images/{contentFile}",
                                              new { controller = "UI" });

            configuration.Routes.MapHttpRoute("attribute",
                                              "{objectName}/{attribute}",
                                              new { controller = "MBeanAttribute" });

            configuration.Routes.MapHttpRoute("bean",
                                              "{objectName}",
                                              new { controller = "MBean" });

            configuration.Routes.MapHttpRoute("server",
                                              "",
                                              new {controller = "MBeanServer"});

        }
    }
}