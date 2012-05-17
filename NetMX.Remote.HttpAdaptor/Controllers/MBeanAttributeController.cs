using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NetMX.OpenMBean;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanAttributeController : BaseController
    {
        public MBeanAttributeController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public HttpResponseMessage<MBeanAttributeResource> Get(string objectName, string attribute)
        {
            try
            {
                var attributeInfo = GetAttributeInfo(objectName, attribute);
                var openType = attributeInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field);

                var resource = BuildResourceRepresentation(objectName, attribute, openType);
                return new HttpResponseMessage<MBeanAttributeResource>(resource);
            }
            catch (AttributeNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (InstanceNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private MBeanAttributeResource BuildResourceRepresentation(string objectName, string attribute, OpenType openType)
        {
            var value = _serverConnection.GetAttribute(objectName, attribute);
            var formattedValue = OpenValueFormatter.FormatValue(openType, value);
            return new MBeanAttributeResource
                       {
                           Value = formattedValue,
                           Name = attribute,
                           MBeanHRef = GetRouteToParent(objectName)
                       };
        }

        public HttpResponseMessage<MBeanAttributeResource> Put(MBeanAttributeResource valueResource)
        {
            try
            {
                var objectName = (string)ControllerContext.RouteData.Values["objectName"];
                var attribute = (string)ControllerContext.RouteData.Values["attribute"];

                var attributeInfo = GetAttributeInfo(objectName, attribute);
                var openType = attributeInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field);

                var value = OpenValueFormatter.ExtractValue(openType, valueResource.Value);
                _serverConnection.SetAttribute(objectName, attribute, value);

                var resource = BuildResourceRepresentation(objectName, attribute, openType);
                return new HttpResponseMessage<MBeanAttributeResource>(resource);
            }
            catch (FormatException ex)
            {
                throw new HttpResponseException(ex.Message, HttpStatusCode.BadRequest);
            }
            catch (InstanceNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private MBeanAttributeInfo GetAttributeInfo(string objectName, string attribute)
        {
            var beanInfo = _serverConnection.GetMBeanInfo(objectName);
            var attributeInfo = beanInfo.Attributes.FirstOrDefault(x => x.Name == attribute);
            if (attributeInfo == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return attributeInfo;
        }

        private string GetRouteToParent(string objectName)
        {
            return GetResourceUrl("bean", new { objectName });
        }


    }
}