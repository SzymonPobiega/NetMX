using System.Linq;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET.Addressing;
using WSMan.NET.Addressing.Faults;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace NetMX.Remote.Jsr262.Server
{
    public class Jsr262ExtensionMethodHandler : AddressingBasedRequestHandler
    {
        private readonly IMBeanServer _server;

        public Jsr262ExtensionMethodHandler(IMBeanServer server)
        {
            _server = server;
        }

        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            switch (actionHeader.Action)
            {
                case Schema.InvokeAction:
                    return Invoke(request);
                case Schema.GetMBeanInfoAction:
                    return GetMBeanInfo(request);
                case Schema.InstanceOfAction:
                    return IsInstanceOf(request);
                default:
                    return null;
            }            
        }

        public OutgoingMessage Invoke(IncomingMessage requestMessage)
        {
            CheckResourceUri(requestMessage, Schema.DynamicMBeanResourceUri);

            var request = requestMessage.GetPayload<InvokeMessage>();
            var selectorSet = requestMessage.GetHeader<SelectorSetHeader>();
            var objectName = selectorSet.ExtractObjectName();
            var arguments = request.ManagedResourceOperation.Input.Select(x => x.Deserialize()).ToArray();

            var result = _server.Invoke(objectName, request.ManagedResourceOperation.name, arguments);

            var response = new InvokeResponseMessage(new GenericValueType(result));
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Schema.InvokeResponseAction), true)
                .SetBody(new SerializerBodyWriter(response));
        }

        public OutgoingMessage GetMBeanInfo(IncomingMessage requestMessage)
        {
            CheckResourceUri(requestMessage, Schema.DynamicMBeanResourceUri);

            var selectorSet = requestMessage.GetHeader<SelectorSetHeader>();
            var objectName = selectorSet.ExtractObjectName();

            var info = _server.GetMBeanInfo(objectName);

            var response = new ResourceMetaDataTypeMessage(new ResourceMetaDataType(info));
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Schema.GetMBeanInfoResponseAction), true)
                .SetBody(new SerializerBodyWriter(response));
        }

        public OutgoingMessage IsInstanceOf(IncomingMessage requestMessage)
        {
            CheckResourceUri(requestMessage, Schema.DynamicMBeanResourceUri);

            var request = requestMessage.GetPayload<IsInstanceOfMessage>();
            var selectorSet = requestMessage.GetHeader<SelectorSetHeader>();
            var objectName = selectorSet.ExtractObjectName();

            //TODO: Java-to-Net class mapping (i.e. javax.management.NotificationBroadcaster)

            var result = _server.IsInstanceOf(objectName, request.Value);
            var response = new IsInstanceOfResponseMessage(result);
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Schema.InstanceOfResponseAction), true)
                .SetBody(new SerializerBodyWriter(response));
        }

        private static void CheckResourceUri(IncomingMessage requestMessage, string expectedResourceUri)
        {
            var resourceUri = requestMessage.GetHeader<ResourceUriHeader>();
            if (resourceUri == null || resourceUri.ResourceUri != expectedResourceUri)
            {
                throw new DestinationUnreachableFaultException();
            }
        }

    }
}