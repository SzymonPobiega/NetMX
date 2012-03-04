using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.SOAP;

namespace NetMX.Remote.Jsr262.Server
{
    public class IsRegisteredEnumerationRequestHandler : IEnumerationRequestHandler
    {
        private readonly IMBeanServer _server;

        public IsRegisteredEnumerationRequestHandler(IMBeanServer server)
        {
            _server = server;
        }

        public IEnumerable<object> Enumerate(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage)
        {
            var name = context.Selectors.ExtractObjectName();
            if (_server.IsRegistered(name))
            {
                yield return ObjectNameSelector.CreateEndpointAddress(name);
            }
        }

        public int EstimateRemainingItemsCount(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage)
        {
            return _server.GetMBeanCount();
        }
    }
}