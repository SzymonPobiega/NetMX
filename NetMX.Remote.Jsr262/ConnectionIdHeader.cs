using System.ServiceModel.Channels;
using System.Xml;

namespace NetMX.Remote.Jsr262
{
    public class ConnectionIdHeader : MessageHeader
    {
        public override string Name
        {
            get { return "ConnectionId"; }
        }

        public override string Namespace
        {
            get { return Schema.ConnectorNamespace; }
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteValue("f0058694-0969-4cf0-9dcb-62c509e4c025");
        }
    }
}