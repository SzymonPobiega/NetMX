//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using System.Text;

//namespace NetMX.Remote.Jsr262
//{
//   public static class WsAddressingFaults
//   {
//      private const string FaultAction = "http://schemas.xmlsoap.org/ws/2004/08/addressing/fault";
//      public const string Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing";

//      private const string EndpointUnavailableCode = "EndpointUnavailable";
//      private const string DestinationUnreachableCode = "DestinationUnreachable";

//      public static bool IsDestinationUnreachable(FaultException exception)
//      {
//         return exception.Action == FaultAction && exception.Code.Name == DestinationUnreachableCode && exception.Code.Namespace == Namespace;
//      }

//      public static FaultException CreateDestinationUnreachable()
//      {
//         return
//            new FaultException(
//               "No route can be determined to reach the destination role defined by the WS-Addressing To.",
//               FaultCode.CreateSenderFaultCode(DestinationUnreachableCode, Namespace),
//               FaultAction);
//      }

//      public static bool IsEndpointUnavailable(FaultException exception)
//      {
//         return exception.Action == FaultAction && exception.Code.Name == EndpointUnavailableCode && exception.Code.Namespace == Namespace;
//      }

//      public static FaultException CreateEndpointUnavailable()
//      {
//         return
//            new FaultException(
//               "The specified endpoint is currently unavailable.",
//               FaultCode.CreateReceiverFaultCode(EndpointUnavailableCode, Namespace),
//               FaultAction);
//      }
//   }
//}