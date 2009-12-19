using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Xml;
using Simon.WsManagement;
using System;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262
{
   [ServiceBehavior(
      ValidateMustUnderstand = false, 
      AutomaticSessionShutdown = false, 
      ConcurrencyMode = ConcurrencyMode.Multiple, 
      InstanceContextMode = InstanceContextMode.Single, 
      IncludeExceptionDetailInFaults = true, 
      AddressFilterMode = AddressFilterMode.Any)]
   [FragmentHeader]
   public class Jsr262ServiceImplementation : IJsr262ServiceContract, ITransferContract
   {
      private readonly IMBeanServer _server;
      private readonly TransferServer _transferServer;

      public Jsr262ServiceImplementation(IMBeanServer server)
      {         
         _server = server;
         _transferServer = new TransferServer(new ManagementServer(new Jsr262ManagementRequestHandler(server)), MessageVersion.Soap12WSAddressingAugust2004);
      }

      public void Dispose()
      { }

      #region INetMXWSService Members                 
      public InvokeResponseMessage Invoke(InvokeMessage request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();
         object[] arguments = request.ManagedResourceOperation.Input.Select(x => x.Deserialize()).ToArray();
         object result = _server.Invoke(objectName, request.ManagedResourceOperation.name, arguments);
         return new InvokeResponseMessage(new GenericValueType(result));
      }

      public ResourceMetaDataTypeMessage GetMBeanInfo()
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         MBeanInfo info = _server.GetMBeanInfo(objectName);

         return new ResourceMetaDataTypeMessage(new ResourceMetaDataType(info));
      }

      public PullResponseMessage Pull(Message request)
      {
         PullResponse response = new PullResponse();
         Thread.Sleep(100000);
         return new PullResponseMessage(response);
         //response.EnumerationContext = "47f64746-2f66-4a46-881f-9ec69dd8c040";
         //return new PullResponseMessage(response);
         //FaultException<string> ex = new FaultException<string>(null, "The supplied enumeration context is invalid.",
         //                         new FaultCode("Receiver", "http://www.w3.org/2003/05/soap-envelope",
         //                                       new FaultCode("InvalidEnumerationContext",
         //                                                     Simon.WsManagement.Schema.Namespace)));

         //return Message.CreateMessage(MessageVersion.Soap12WSAddressingAugust2004, ex.CreateMessageFault(), "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault");
      }

      public EnumerateResponseMessage Enumerate(Message request)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         MessageHeaders incomingMessageHeaders = OperationContext.Current.IncomingMessageHeaders;
         bool countRequest =
           RequestTotalItemsTotalCountEstimate.IsPresent(incomingMessageHeaders);
         if (countRequest)
         {
            int mbeanCount = _server.GetMBeanCount();
            TotalItemsTotalCountEstimate responseHeader = new TotalItemsTotalCountEstimate(mbeanCount);
            OperationContext.Current.OutgoingMessageHeaders.Add(responseHeader);
            return new EnumerateResponseMessage();
         }
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(incomingMessageHeaders);
         ObjectName objectName = null;
         if (selectorSet != null)
         {
            objectName = selectorSet.ExtractObjectName();
         }

         if (objectName != null)
         {
            List<EndpointAddress> result = new List<EndpointAddress>();
            if (_server.IsRegistered(objectName))
            {
               result.Add(CreateObjectNameEPR(objectName));
            }
            return new EnumerateResponseMessage(new EnumerateResponse(result));
         }

         //            Message request = OperationContext.Current.RequestContext.RequestMessage;
         MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
         request = buffer.CreateMessage();
         //            Message originalMessage = buffer.CreateMessage();

         XmlDictionaryReader reader = request.GetReaderAtBodyContents();
         XmlDocument doc = new XmlDocument();
         doc.Load(reader);
         //            reader.ReadInnerXml();
         XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
         ns.AddNamespace("wsman", Schema.ManagementNamespace);
         XmlNode node = doc.SelectSingleNode("//wsman:Filter/@Dialect", ns);

         string dialect = null; // request.Filter.Dialect;
         if (node != null)
         {
            dialect = node.Value;
         }
         if (dialect == Schema.QueryNamesDialect)
         {
            return new EnumerateResponseMessage(new EnumerateResponse(_server.QueryNames(objectName, null).Select(x => CreateObjectNameEPR(x))));
         }
         throw new NotSupportedException();
      }

      private static EndpointAddress CreateObjectNameEPR(ObjectName objectName)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         builder.Headers.Add(ObjectNameSelector.CreateSelectorSetHeader(objectName));
         builder.Uri = OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri;
         builder.Headers.Add(new ResourceUriHeader(Schema.DynamicMBeanResourceUri));
         return builder.ToEndpointAddress();
      }

      public IsInstanceOfResponseMessage IsInstanceOf(IsInstanceOfMessage className)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         //TODO: Java-to-Net class mapping (i.e. javax.management.NotificationBroadcaster)

         return new IsInstanceOfResponseMessage(_server.IsInstanceOf(objectName, className.Value));
      }

      public Message Subscribe(Message msg)
      {
         string xml = @"<SubscribeResponse xmlns='http://schemas.xmlsoap.org/ws/2004/08/eventing'>
              <EnumerationContext xmlns='http://schemas.xmlsoap.org/ws/2004/09/enumeration'>47f64746-2f66-4a46-881f-9ec69dd8c040</EnumerationContext>
              <SubscriptionManager>
                <Address xmlns='http://schemas.xmlsoap.org/ws/2004/08/addressing'>http://0.0.0.0:9998/jmxws</Address>
                <ReferenceParameters xmlns='http://schemas.xmlsoap.org/ws/2004/08/addressing'>
                  <NotificationListenerList xmlns='http://jsr262.dev.java.net/jmxconnector'>0</NotificationListenerList>
                  <ResourceURI xmlns='http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd'>http://jsr262.dev.java.net/MBeanNotificationSubscriptionManager</ResourceURI>
                  <Identifier xmlns='http://schemas.xmlsoap.org/ws/2004/08/eventing'>47f64746-2f66-4a46-881f-9ec69dd8c040</Identifier>
                </ReferenceParameters>
               </SubscriptionManager>                
            </SubscribeResponse>";
         XmlDocument xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(xml);

         Message response = Message.CreateMessage(MessageVersion.Soap12WSAddressingAugust2004, Schema.SubscribeResponseAction,
                               new XmlNodeReader(xmlDoc.DocumentElement));
         return response;
         //            FaultException ex = new FaultException("The requested delivery mode is not supported.",
         //                                     new FaultCode("Sender", "http://www.w3.org/2003/05/soap-envelope",
         //                                                   new FaultCode("DeliveryModeRequestedUnavailable",
         //                                                                 Simon.WsManagement.Schema.EventsNamespace)));
         //            
         //            return Message.CreateMessage(MessageVersion.Soap12WSAddressingAugust2004, ex.CreateMessageFault(), "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault");
      }

      public void Unsubscribe(Message msg)
      { }

      #endregion

      private static void CheckResourceUri(string expectedResourceUri)
      {
         ResourceUriHeader resourceUri = ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         if (resourceUri == null || resourceUri.ResourceUri != expectedResourceUri)
         {
            throw WsAddressing.CreateDestinationUnreachable();
         }
      }

      public Message Get(Message getRequest)
      {
         return _transferServer.Get(getRequest);
      }

      public Message Put(Message putRequest)
      {
         return _transferServer.Put(putRequest);
      }

      public Message Create(Message createRequest)
      {
         return _transferServer.Create(createRequest);
      }

      public Message Delete(Message deleteRequest)
      {
         return _transferServer.Delete(deleteRequest);
      }
   }
}