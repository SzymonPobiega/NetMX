using System;
using System.Net;
using System.Net.Http;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class RootController : BaseController
    {
        public RootController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse(HttpStatusCode.SeeOther);
            response.Headers.Location = new Uri(new Uri(_baseUrl), GetResourceUrl("server",new {}));
            return response;
        }
    }
}