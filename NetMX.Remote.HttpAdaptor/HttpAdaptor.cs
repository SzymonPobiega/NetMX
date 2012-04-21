using System.Web.Http;

namespace NetMX.Remote.HttpAdaptor
{
    public class HttpAdaptor
    {
        protected void Configure(HttpConfiguration configuration)
        {            
            configuration.Routes.MapHttpRoute("server", 
                "",
                new { controller = "MBeanServer" });
        }
    }
}