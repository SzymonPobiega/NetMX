using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Simon.WsManagement
{
   public static class WsAddressing
   {
      private const string FaultAction = "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault";
      private const string Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing";

      public static FaultException CreateDestinationUnreachable()
      {
         return
            new FaultException(
               "No route can be determined to reach the destination role defined by the WS-Addressing To.",
               FaultCode.CreateSenderFaultCode("DestinationUnreachable", Namespace),
               FaultAction);
      }
   }
}
