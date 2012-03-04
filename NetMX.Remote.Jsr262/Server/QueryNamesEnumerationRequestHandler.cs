using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.SOAP;

namespace NetMX.Remote.Jsr262.Server
{
   public class QueryNamesEnumerationRequestHandler : IEnumerationRequestHandler
   {
      private readonly IMBeanServer _server;

      public QueryNamesEnumerationRequestHandler(IMBeanServer server)
      {
         _server = server;
      }

      public IEnumerable<object> Enumerate(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage)
      {
         //QueryExpr expr = (QueryExpr) context.Filter;
         return _server.QueryNames(context.Selectors.ExtractObjectName(), null)
            .Select(ObjectNameSelector.CreateEndpointAddress).Cast<object>();
      }

      public int EstimateRemainingItemsCount(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage)
      {
         throw new NotSupportedException();
      }      
   }
}