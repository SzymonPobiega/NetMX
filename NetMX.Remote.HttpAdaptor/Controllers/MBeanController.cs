using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanController : BaseController
    {
        public MBeanController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public HttpResponseMessage<MBeanResource> Get(string objectName)
        {
            try
            {
                var info = _serverConnection.GetMBeanInfo(objectName);
                var resource = new MBeanResource
                                   {
                                       ClassName = info.ClassName,
                                       Description = info.Description,
                                       Attributes = MapAttributes(objectName, info.Attributes),
                                       ServerHRef = GetResourceUrl("server", new {})
                                   };
                return new HttpResponseMessage<MBeanResource>(resource);
            }
            catch (InstanceNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private List<Resources.MBeanAttributeInfo> MapAttributes(string objectName, IEnumerable<MBeanAttributeInfo> attributes)
        {
            return attributes
                .Select(x => new Resources.MBeanAttributeInfo
                                 {
                                     Name = x.Name,
                                     HRef = GetResourceUrl("attribute", new { objectName, attribute = x.Name })
                                 })
                .ToList();
        }
    }
}