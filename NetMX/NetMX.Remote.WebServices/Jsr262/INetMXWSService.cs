using System.ServiceModel;
using System.ServiceModel.Channels;

namespace NetMX.Remote.WebServices.Jsr262
{
    [ServiceContract]
    [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
    public interface INetMXWSService
    {
        [OperationContract]
        Message GetAttribute(Message request);
    }
}