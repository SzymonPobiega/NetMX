using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
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
                var value = _serverConnection.GetAttribute(objectName, attribute);
                var resource = FormatValue(attributeInfo, value);
                resource.Name = attribute;
                resource.MBeanHRef = GetRouteToParent(objectName);
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

        public HttpResponseMessage<MBeanAttributeResource> Put(MBeanAttributeResource valueResource)
        {
            try
            {
                var objectName = (string) ControllerContext.RouteData.Values["objectName"];
                var attribute = (string) ControllerContext.RouteData.Values["attribute"];

                var attributeInfo = GetAttributeInfo(objectName, attribute);
                var value = ExtractValue(attributeInfo, valueResource);
                _serverConnection.SetAttribute(objectName, attribute, value);
                var currentValue = _serverConnection.GetAttribute(objectName, attribute);
                var resource = FormatValue(attributeInfo, currentValue);
                resource.Name = attribute;
                resource.MBeanHRef = GetRouteToParent(objectName);
                return new HttpResponseMessage<MBeanAttributeResource>(resource);
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

        private static object ExtractValue(MBeanAttributeInfo attributeInfo, MBeanAttributeResource valueResource)
        {
            var openType = attributeInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field);
            if (openType.Kind == OpenTypeKind.SimpleType)
            {
                var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
                var simpleValueResource = (MBeanSimpleValueAttributeResource)valueResource;
                return typeConverter.ConvertFromString(simpleValueResource.SimpleValue);
            }
            var complexValueResource = (MBeanComplexValueAttributeResource)valueResource;
            return complexValueResource.ComplexValue;
        }

        private static MBeanAttributeResource FormatValue(MBeanAttributeInfo attributeInfo, object value)
        {
            var openType = attributeInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field);
            if (openType.Kind == OpenTypeKind.SimpleType)
            {
                var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
                return new MBeanSimpleValueAttributeResource
                           {
                               Type = openType.TypeName,
                               SimpleValue = typeConverter.ConvertToString(value)
                           };
            }
            return new MBeanComplexValueAttributeResource
                       {
                           ComplexValue = value
                       };
        }
    }
}