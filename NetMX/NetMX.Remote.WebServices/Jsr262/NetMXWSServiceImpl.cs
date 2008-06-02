using System.ServiceModel;
using System.ServiceModel.Channels;
using NetMX.Remote.WebServices.WsAddressing;
using NetMX.Remote.WebServices.WSManagement;
using System;

namespace NetMX.Remote.WebServices.Jsr262
{
    internal class NetMXWSServiceImpl : INetMXWSService
    {
        private IMBeanServer _server;

        #region INetMXWSService Members
        public Message GetAttribute(Message request)
        {            
            string resourceUri = ResourceUriHeader.ReadFrom(request);
            if (resourceUri == null || resourceUri != Consts.DynamicMBeanResourceUri)
            {
                return Faults.CreateDestinationUnreachable();
            }
            
            //FragmentTransferHeader fragmentTransfer = FragmentTransferHeader.ReadFrom(request);
            WsmanSelectorSetHeader selectorSet = WsmanSelectorSetHeader.ReadFrom(request);
            //selectorSet.Selectors

            DynamicMBeanResource resource = new DynamicMBeanResource();

            Message response = Message.CreateMessage(MessageVersion.Soap12WSAddressingAugust2004,
                                                     WsTransfer.Const.GetResponseAction, resource);
            return response;
        }        
        #endregion
    }
}