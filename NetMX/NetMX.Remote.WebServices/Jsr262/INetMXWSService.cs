using System.ServiceModel;
using System.ServiceModel.Channels;
using NetMX.Remote.WebServices.WSManagement;

namespace NetMX.Remote.WebServices.Jsr262
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
   public interface INetMXWSService
   {
      [OperationContract(Action = WsTransfer.Const.GetAction,
         ReplyAction = WsTransfer.Const.GetResponseAction)]
      DynamicMBeanResource GetAttributes();

      [OperationContract(Action = @"http://schemas.xmlsoap.org/ws/2004/09/transfer/Put")]
      DynamicMBeanResource SetAttribute();

      [OperationContract(Action = @"http://jsr262.dev.java.net/DynamicMBeanResource/Invoke")]
      void Invoke();
   }
}