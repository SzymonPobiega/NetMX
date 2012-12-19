using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NetMX.Relation;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class MBeanController : BaseController
    {
        public MBeanController(IMBeanServerConnection serverConnection, string baseUrl)
            : base(serverConnection, baseUrl)
        {
        }

        public MBeanResource Get(string objectName)
        {
            try
            {
                var info = _serverConnection.GetMBeanInfo(objectName);

                var relationBean = _serverConnection.CreateDynamicProxy(RelationService.ObjectName);
                IDictionary<ObjectName, IList<string>> related = relationBean.FindAssociatedMBeans((ObjectName)objectName, null, null);

                var resource = new MBeanResource
                                   {
                                       ClassName = info.ClassName,
                                       Description = info.Description,
                                       Attributes = MapAttributes(objectName, info.Attributes),
                                       ServerHRef = GetResourceUrl("server", new {}),
                                       Relations = related.SelectMany(MapRelation).ToList()
                                   };
                //response.StatusCode = HttpStatusCode.OK;
                return resource;
            }
            catch (InstanceNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private IEnumerable<MBeanRelationInfo> MapRelation(KeyValuePair<ObjectName, IList<string>> relationData)
        {
            var relationBean = _serverConnection.CreateDynamicProxy(RelationService.ObjectName);
            return relationData.Value.Select(x =>
                                                 {
                                                     string typeName = relationBean.GetRelationTypeName(x);
                                                     return new MBeanRelationInfo
                                                                {
                                                                    ObjectName = relationData.Key,
                                                                    RelationType = typeName,
                                                                    HRef = GetResourceUrl("bean", new { objectName = (string)relationData.Key })
                                                                };
                                                 });
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