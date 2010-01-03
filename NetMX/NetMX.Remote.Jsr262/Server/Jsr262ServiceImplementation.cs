using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Xml;
using System;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262.Server
{
   [FragmentHeader]
   public class Jsr262ServiceImplementation : ManagementServer, IJsr262ServiceContract
   {
      private readonly IMBeanServer _server;

      public Jsr262ServiceImplementation(IMBeanServer server)
      {         
         _server = server;

         BindManagement(new Uri(Schema.MBeanServerResourceUri), new MBeanServerManagementRequestHandler(server));
         BindManagement(new Uri(Schema.DynamicMBeanResourceUri), new DynamicMBeanManagementRequestHandler(server));

         BindEnumeration(new Uri(Schema.DynamicMBeanResourceUri), Schema.QueryNamesDialect, typeof(QueryExpr), new QueryNamesEnumerationRequestHandler(server));
         BindEnumeration(new Uri(Schema.DynamicMBeanResourceUri), FilterMap.DefaultDialect, typeof(void), new IsRegisteredEnumerationRequestHandler(server));

         BindPullEventing(new Uri(Schema.DynamicMBeanResourceUri), Schema.NotificationDialect, typeof(NotificationFilter), new EventingRequestHandler(server), new Uri(Schema.MBeanNotificationSubscriptionManagerUri));
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
            
      public IsInstanceOfResponseMessage IsInstanceOf(IsInstanceOfMessage className)
      {
         CheckResourceUri(Schema.DynamicMBeanResourceUri);

         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         ObjectName objectName = selectorSet.ExtractObjectName();

         //TODO: Java-to-Net class mapping (i.e. javax.management.NotificationBroadcaster)

         return new IsInstanceOfResponseMessage(_server.IsInstanceOf(objectName, className.Value));
      }
      #endregion

      private static void CheckResourceUri(string expectedResourceUri)
      {
         ResourceUriHeader resourceUri = ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         if (resourceUri == null || resourceUri.ResourceUri != expectedResourceUri)
         {
            throw WsAddressingFaults.CreateDestinationUnreachable();
         }
      }      
   }
}