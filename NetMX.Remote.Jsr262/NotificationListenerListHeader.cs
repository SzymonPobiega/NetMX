using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace NetMX.Remote.Jsr262
{
    public class NotificationListenerListHeader : IMessageHeader
    {
        private string _value;

        public NotificationListenerListHeader()
        {
        }

        public NotificationListenerListHeader(string value)
        {
            _value = value;
        }

        public XName Name
        {
            get { return Schema.ConnectorNamespace + "NotificationListenerList"; }
        }

        public string Value
        {
            get { return _value; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Value);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _value = text.Value;
        }

    }
}
