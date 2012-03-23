using System;
using System.Linq;
using System.Collections.Generic;
using NetMX.Expression;
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
            var filterExpr = context.Filter != null 
                ? ExpressionParser.Parse<bool>((string)context.Filter)
                : null;

            return _server
                .QueryNames(context.Selectors.ExtractObjectName(), filterExpr)
                .Select(ObjectNameSelector.CreateEndpointAddress);
        }

        public int EstimateRemainingItemsCount(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage)
        {
            throw new NotSupportedException();
        }
    }
}