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
      public DynamicMBeanResource GetAttributes()
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         FragmentTransferHeader fragmentTransfer = FragmentTransferHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);         
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         GetAttributesFragment typedFragment = GetAttributesFragment.Parse(fragmentTransfer.Expression);
         
         DynamicMBeanResource response = new DynamicMBeanResource();
         ObjectName objectName = selectorSet.ExtractObjectName();

         IList<AttributeValue> values = _server.GetAttributes(objectName, typedFragment.Names);

         response.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();

         OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransfer);         
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
      public GenericValueType Invoke(OperationRequestType request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();
         object[] arguments = request.Input.Select(x => x.Deserialize()).ToArray();
         object result = _server.Invoke(objectName, request.name, arguments);
         return new GenericValueType(result);
      }

      public ResourceCreated CreateMBean(DynamicMBeanResourceConstructor request)
      {
         CheckResourceUri(Schema.MBeanServerResourceUri);

         ObjectName objectName = request.ResourceEPR.ObjectName;
         object[] arguments = request.RegistrationParameters.Select(x => x.Deserialize()).ToArray();

         ObjectInstance instance = _server.CreateMBean(request.ResourceClass, objectName, arguments);

         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         builder.Headers.Add(ObjectNameSelector.CreateSelectorSet(instance.ObjectName));         
         return new ResourceCreated(builder.ToEndpointAddress());
      }

      public ResourceMetaDataType GetMBeanInfo()
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         MBeanInfo info = _server.GetMBeanInfo(objectName);

         return new ResourceMetaDataType(info);
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