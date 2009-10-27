using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Simon.WsManagement;
using System;

namespace NetMX.Remote.Jsr262
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class Jsr262ServiceImplementation : IJsr262ServiceContract
   {
      private readonly IMBeanServer _server;

      public Jsr262ServiceImplementation(IMBeanServer server)
      {
         _server = server;
      }

      #region INetMXWSService Members      
      public GetResponse Get()
      {
         GetResponse result = new GetResponse();
         FragmentTransferHeader fragmentTransfer = FragmentTransferHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);         
         if (fragmentTransfer.Expression == IJsr262ServiceContractConstants.GetDefaultDomainFragmentTransferPath)
         {
            result.GetDefaultDomainResponse = GetDefaultDomain();
         }
         else if (fragmentTransfer.Expression == IJsr262ServiceContractConstants.GetDomainsFragmentTransferPath)
         {
            result.GetDomainsResponse = GetDomains();
         }
         else
         {
            result.DynamicMBeanResource = GetAttributes(fragmentTransfer.Expression);  
         }         
         OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransfer);
         return result;
      }

      private GetDomainsResponse GetDomains()
      {
         return new GetDomainsResponse {DomainNames = _server.GetDomains().ToArray()};
      }

      private GetDefaultDomainResponse GetDefaultDomain()
      {
         return new GetDefaultDomainResponse {DomainName = _server.GetDefaultDomain()};
      }

      private DynamicMBeanResource GetAttributes(string fragmentTransferExpression)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);
         
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         GetAttributesFragment typedFragment = GetAttributesFragment.Parse(fragmentTransferExpression);
         
         DynamicMBeanResource response = new DynamicMBeanResource();
         ObjectName objectName = selectorSet.ExtractObjectName();

         IList<AttributeValue> values = _server.GetAttributes(objectName, typedFragment.Names);

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();                  
         return response;
      }

      public DynamicMBeanResource SetAttributes(DynamicMBeanResource request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);
         
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);

         DynamicMBeanResource response = new DynamicMBeanResource();
         ObjectName objectName = selectorSet.ExtractObjectName();

         IList<AttributeValue> values = _server.SetAttributes(objectName, request.Property.Select(x => new AttributeValue(x.name, x.Deserialize())));

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();         
         return response;
      }

      public void UnregisterMBean()
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         try
         {
            _server.UnregisterMBean(objectName);
         }
         catch (InstanceNotFoundException)
         {
            throw WsAddressing.CreateEndpointUnavailable();
         }         
      }

      public GenericValueType Invoke(OperationRequestType request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();
         object[] arguments = request.Input.Select(x => x.Deserialize()).ToArray();
         object result = _server.Invoke(objectName, request.name, arguments);
         return new GenericValueType(result);
      }

      public EndpointReferenceType CreateMBean(DynamicMBeanResourceConstructor request)
      {
         CheckResourceUri(Schema.MBeanServerResourceUri);

         ObjectName objectName = request.ResourceEPR.ObjectName;
         object[] arguments = request.RegistrationParameters.Select(x => x.Deserialize()).ToArray();

         ObjectInstance instance = _server.CreateMBean(request.ResourceClass, objectName, arguments);

         //EndpointAddressBuilder builder = new EndpointAddressBuilder();
         //builder.Headers.Add(ObjectNameSelector.CreateSelectorSet(instance.ObjectName));
         return new EndpointReferenceType(instance.ObjectName);
      }

      public ResourceMetaDataType GetMBeanInfo()
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         MBeanInfo info = _server.GetMBeanInfo(objectName);

         return new ResourceMetaDataType(info);
      }

      public EnumerateResponse Enumerate(Enumerate request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         bool countRequest =
            RequestTotalItemsTotalCountEstimate.IsPresent(OperationContext.Current.IncomingMessageHeaders);
         if (countRequest)
         {
            int mbeanCount = _server.GetMBeanCount();
            TotalItemsTotalCountEstimate responseHeader = new TotalItemsTotalCountEstimate(mbeanCount);
            OperationContext.Current.OutgoingMessageHeaders.Add(responseHeader);
            return new EnumerateResponse();
         }
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = null;
         if (selectorSet != null)
         {
            objectName = selectorSet.ExtractObjectName();
         }

         if (request.Filter == null)
         {
            List<EndpointAddress> result = new List<EndpointAddress>();
            if (_server.IsRegistered(objectName))
            {
               result.Add(CreateObjectNameEPR(objectName));
            }
            return new EnumerateResponse(result);
         }
         string dialect = request.Filter.Dialect;
         if (dialect == Schema.QueryNamesDialect)
         {            
            return new EnumerateResponse(_server.QueryNames(objectName, null).Select(x => CreateObjectNameEPR(x)));
         }
         throw new NotSupportedException();
      }

      private static EndpointAddress CreateObjectNameEPR(ObjectName objectName)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         builder.Headers.Add(ObjectNameSelector.CreateSelectorSet(objectName));
         builder.Uri = OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri;
         builder.Headers.Add(new ResourceUriHeader(Schema.DynamicMBeanResourceUri));
         return builder.ToEndpointAddress();
      }

      public GenericValueType IsInstanceOf(GenericValueType className)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         if (className.ItemElementName != ItemChoiceType.String)
         {
            throw new InvalidOperationException();
         }
         string name = (string)className.Deserialize();
         return new GenericValueType(_server.IsInstanceOf(objectName, name));
      }

      #endregion

      private static void CheckResourceUri(string expectedResourceUri)
      {
         ResourceUriHeader resourceUri = ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         if (resourceUri == null || resourceUri.ResourceUri != expectedResourceUri)
         {
            throw WsAddressing.CreateDestinationUnreachable();
         }
      }
   }
}