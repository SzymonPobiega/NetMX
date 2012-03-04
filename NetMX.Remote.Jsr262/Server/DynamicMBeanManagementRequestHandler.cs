using System;
using System.Collections.Generic;
using System.Linq;
using WSMan.NET.Addressing;
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
         var typedFragment = GetAttributesFragment.Parse(fragmentExpression);

         var response = new DynamicMBeanResource();
         var objectName = selectors.ExtractObjectName();

         var values = _server.GetAttributes(objectName, typedFragment.Names);

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();
         return new XmlFragment<DynamicMBeanResource>(response);
      }

      public object HandlePut(string fragmentExpression, IEnumerable<Selector> selectors, ExtractBodyDelegate extractBodyCallback)
      {
         var response = new DynamicMBeanResource();
         var objectName = selectors.ExtractObjectName();

         var request = (XmlFragment<DynamicMBeanResource>)extractBodyCallback(typeof(XmlFragment<DynamicMBeanResource>));

         var values = _server.SetAttributes(objectName, request.Value.Property.Select(x => new AttributeValue(x.name, x.Deserialize())));

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();
         return new XmlFragment<DynamicMBeanResource>(response);
      }

      public EndpointReference HandleCreate(ExtractBodyDelegate extractBodyCallback)
      {
         throw new NotSupportedException();
      }

      public void HandlerDelete(IEnumerable<Selector> selectors)
      {
         var objectName = selectors.ExtractObjectName();
         _server.UnregisterMBean(objectName);
      }
   }
}