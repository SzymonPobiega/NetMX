using System.Web.Http;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IMBeanServerConnection _serverConnection;
        protected readonly string _baseUrl;

        protected BaseController(IMBeanServerConnection serverConnection, string baseUrl)
        {
            _serverConnection = serverConnection;
            _baseUrl = baseUrl;
        }

        protected string GetResourceUrl(string name, object routeData)
        {
            return _baseUrl + "/" +  Url.Route(name, routeData);
        }
    }
}