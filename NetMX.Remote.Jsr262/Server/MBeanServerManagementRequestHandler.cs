using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Addressing;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262.Server
{
   public class MBeanServerManagementRequestHandler : IManagementRequestHandler
   {
      private readonly IMBeanServer _server;

      public MBeanServerManagementRequestHandler(IMBeanServer beanServer)
      {
         _server = beanServer;
      }

      public bool CanHandle(string resourceUri)
      {
         return true;
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
         throw new NotSupportedException();         
      }

      private XmlFragment<GetDomainsResponse> GetDomains()
      {
         return new XmlFragment<GetDomainsResponse>(new GetDomainsResponse { DomainNames = _server.GetDomains().ToArray() });
      }

      private XmlFragment<GetDefaultDomainResponse> GetDefaultDomain()
      {
         return new XmlFragment<GetDefaultDomainResponse>(new GetDefaultDomainResponse { DomainName = _server.GetDefaultDomain() });
      }      

      public object HandlePut(string fragmentExpression, IEnumerable<Selector> selectors, ExtractBodyDelegate extractBodyCallback)
      {
         throw new NotSupportedException();
      }

      public EndpointReference HandleCreate(ExtractBodyDelegate extractBodyCallback)
      {
         var request = (DynamicMBeanResourceConstructor)extractBodyCallback(typeof(DynamicMBeanResourceConstructor));

         var objectName = request.ResourceEPR.ExtractObjectName();
         var arguments = request.RegistrationParameters.Select(x => x.Deserialize()).ToArray();

         var instance = _server.CreateMBean(request.ResourceClass, objectName, arguments);

         return ObjectNameSelector.CreateEndpointAddress(instance.ObjectName);
      }

      public void HandlerDelete(IEnumerable<Selector> selectors)
      {
         throw new NotSupportedException();         
      }
   }
}