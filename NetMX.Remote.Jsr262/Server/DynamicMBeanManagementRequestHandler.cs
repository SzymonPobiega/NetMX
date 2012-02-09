using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262.Server
{
   public class DynamicMBeanManagementRequestHandler : IManagementRequestHandler
   {
      private readonly IMBeanServer _server;

      public DynamicMBeanManagementRequestHandler(IMBeanServer beanServer)
      {
         _server = beanServer;
      }

      public bool CanHandle(string resourceUri)
      {
         return true;
      }

      public object HandleGet(string fragmentExpression, IEnumerable<Selector> selectors)
      {
         GetAttributesFragment typedFragment = GetAttributesFragment.Parse(fragmentExpression);

         DynamicMBeanResource response = new DynamicMBeanResource();
         ObjectName objectName = selectors.ExtractObjectName();

         IList<AttributeValue> values = _server.GetAttributes(objectName, typedFragment.Names);

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();
         return new XmlFragment<DynamicMBeanResource>(response);
      }

      public object HandlePut(string fragmentExpression, IEnumerable<Selector> selectors, ExtractBodyDelegate extractBodyCallback)
      {
         DynamicMBeanResource response = new DynamicMBeanResource();
         ObjectName objectName = selectors.ExtractObjectName();

         XmlFragment<DynamicMBeanResource> request = (XmlFragment<DynamicMBeanResource>)extractBodyCallback(typeof(XmlFragment<DynamicMBeanResource>));

         IList<AttributeValue> values = _server.SetAttributes(objectName, request.Value.Property.Select(x => new AttributeValue(x.name, x.Deserialize())));

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();
         return new XmlFragment<DynamicMBeanResource>(response);
      }

      public EndpointAddress HandleCreate(ExtractBodyDelegate extractBodyCallback)
      {
         throw new NotSupportedException();
      }

      public void HandlerDelete(IEnumerable<Selector> selectors)
      {
         ObjectName objectName = selectors.ExtractObjectName();
         _server.UnregisterMBean(objectName);
      }
   }
}