using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262
{
   public class Jsr262ManagementRequestHandler : IManagementRequestHandler
   {
      private readonly IMBeanServer _server;

      public Jsr262ManagementRequestHandler(IMBeanServer beanServer)
      {
         _server = beanServer;
      }

      public bool CanHandle(string resourceUri)
      {
         return true; // resourceUri == Schema.DynamicMBeanResourceUri;
      }

      public object HandleGet(string fragmentExpression, IEnumerable<Selector> selectors)
      {
         if (fragmentExpression.EndsWith(IJsr262ServiceContractConstants.GetDefaultDomainFragmentTransferPath))
         {
            return GetDefaultDomain();
         }
         if (fragmentExpression == IJsr262ServiceContractConstants.GetDomainsFragmentTransferPath)
         {
            return GetDomains();
         }
         {
            return GetAttributes(fragmentExpression, selectors);
         }                  
      }

      private XmlFragment<GetDomainsResponse> GetDomains()
      {
         return new XmlFragment<GetDomainsResponse>(new GetDomainsResponse { DomainNames = _server.GetDomains().ToArray() });
      }

      private XmlFragment<GetDefaultDomainResponse> GetDefaultDomain()
      {
         return new XmlFragment<GetDefaultDomainResponse>(new GetDefaultDomainResponse { DomainName = _server.GetDefaultDomain() });
      }

      private XmlFragment<DynamicMBeanResource> GetAttributes(string fragmentTransferExpression, IEnumerable<Selector> selectors)
      {
         GetAttributesFragment typedFragment = GetAttributesFragment.Parse(fragmentTransferExpression);

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
         DynamicMBeanResourceConstructor request = (DynamicMBeanResourceConstructor)extractBodyCallback(typeof(DynamicMBeanResourceConstructor));

         ObjectName objectName = request.ResourceEPR.ObjectName;
         object[] arguments = request.RegistrationParameters.Select(x => x.Deserialize()).ToArray();

         ObjectInstance instance = _server.CreateMBean(request.ResourceClass, objectName, arguments);

         return ObjectNameSelector.CreateEndpointAddress(instance.ObjectName);
      }

      public void HandlerDelete(IEnumerable<Selector> selectors)
      {
         ObjectName objectName = selectors.ExtractObjectName();
         _server.UnregisterMBean(objectName);
      }
   }
}
