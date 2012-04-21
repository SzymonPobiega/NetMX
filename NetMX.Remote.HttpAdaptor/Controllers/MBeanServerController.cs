using System.Net.Http;
using System.Web.Http;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanServerController : ApiController
    {
        public HttpResponseMessage<MBeanServerResource> Get()
        {
            return new HttpResponseMessage<MBeanServerResource>(null);
        }
    }
}