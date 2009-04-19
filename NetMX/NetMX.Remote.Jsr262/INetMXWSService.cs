using System.ServiceModel;
using System.ServiceModel.Channels;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
   public interface INetMXWSService
   {
      [OperationContract(Action = WsTransfer.GetAction,
         ReplyAction = WsTransfer.GetResponseAction)]
      DynamicMBeanResource GetAttributes();

      [OperationContract(Action = @"http://schemas.xmlsoap.org/ws/2004/09/transfer/Put")]
      DynamicMBeanResource SetAttribute();

      [OperationContract(Action = @"http://jsr262.dev.java.net/DynamicMBeanResource/Invoke")]
      void Invoke();
   }
}