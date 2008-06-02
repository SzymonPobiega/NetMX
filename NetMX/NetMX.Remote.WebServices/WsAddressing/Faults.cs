using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace NetMX.Remote.WebServices.WsAddressing
{
    public static class Faults
    {
        private const string FaultAction = "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault";        

        public static Message CreateDestinationUnreachable()
        {
            return Message.CreateMessage(MessageVersion.Soap12WSAddressingAugust2004,
                                         FaultCode.CreateSenderFaultCode("DestinationUnreachable",
                                                                           Consts.WsAddressingNamespace),
                                         "No route can be determined to reach the destination role defined by the WS-Addressing To.",
                                         "http://schemas.dmtf.org/wbem/wsman/1/wsman/faultDetail/InvalidResourceURI",
                                         FaultAction);            
        }                
    }
}
