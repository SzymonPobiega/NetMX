using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NetMX.Remote.WebServices.WsAddressing;
using NetMX.Remote.WebServices.WSManagement;
using System;
using NetMX.Remote.WebServices.WSManagement.FragmentTransfer;

namespace NetMX.Remote.WebServices.Jsr262
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
   public class NetMXWSServiceImpl : INetMXWSService
   {
      private readonly IMBeanServer _server;

      public NetMXWSServiceImpl(IMBeanServer server)
      {
         _server = server;
      }

      #region INetMXWSService Members      
      public DynamicMBeanResource GetAttributes()
      {
         ResourceUriHeader resourceUri = ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         if (resourceUri == null || resourceUri.ResourceUri != Consts.DynamicMBeanResourceUri)
         {            
            throw Faults.CreateDestinationUnreachable();
         }

         FragmentTransferHeader fragmentTransfer = FragmentTransferHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         GetAttributesFragment typedFragment = GetAttributesFragment.Parse(fragmentTransfer.Expression);
         SelectorSetHeader selectorSet = SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);         
         
         DynamicMBeanResource resource = new DynamicMBeanResource();
         ObjectName objectName = selectorSet.Selectors[0].SimpleValue;

         IList<AttributeValue> values = _server.GetAttributes(objectName, typedFragment.Names);

         resource.Property = values.Select(x => new NamedGenericValueType(x.Name, x.Value)).ToArray();

         Message response = Message.CreateMessage(MessageVersion.Soap12WSAddressing10,
                                                  WsTransfer.Const.GetResponseAction, resource);
         response.Headers.Add(fragmentTransfer);

         return resource;
      }
      public DynamicMBeanResource SetAttribute()
      {
         throw new NotImplementedException();
      }
      public void Invoke()
      {
         throw new NotImplementedException();
      }
      #endregion
   }
}