using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262.Server
{
   public class QueryNamesEnumerationRequestHandler : IEnumerationRequestHandler
   {
      private readonly IMBeanServer _server;

      public QueryNamesEnumerationRequestHandler(IMBeanServer server)
      {
         _server = server;
      }

      public IEnumerable<object> Enumerate(IEnumerationContext context)
      {
         //QueryExpr expr = (QueryExpr) context.Filter;
         return _server.QueryNames(context.Selectors.ExtractObjectName(), null)
            .Select(x => ObjectNameSelector.CreateEndpointAddress(x)).Cast<object>();
      }

      public int EstimateRemainingItemsCount(IEnumerationContext context)
      {
         throw new NotSupportedException();
      }      
   }
}