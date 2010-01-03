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

      public static FaultException CreateDestinationUnreachable()
      {
         return
            new FaultException(
               "No route can be determined to reach the destination role defined by the WS-Addressing To.",
               FaultCode.CreateSenderFaultCode("DestinationUnreachable", Consts.WsAddressingNamespace),
               FaultAction);         
      }
   }
}
