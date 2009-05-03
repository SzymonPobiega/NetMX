using System.ServiceModel;
using System.ServiceModel.Channels;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
   public interface IJsr262ServiceContract
   {
      [OperationContract(Action = WsTransfer.GetAction,
         ReplyAction = WsTransfer.GetResponseAction)]
      DynamicMBeanResource GetAttributes();

      [OperationContract(Action = WsTransfer.PutAction,
         ReplyAction = WsTransfer.PutResponseAction)]
      DynamicMBeanResource SetAttributes(DynamicMBeanResource request);

      [OperationContract(Action = Schema.InvokeAction,
         ReplyAction = Schema.InvokeResponseAction)]
      GenericValueType Invoke(OperationRequestType request);

      [OperationContract(Action = WsTransfer.CreateAction,
         ReplyAction = WsTransfer.CreateResponseAction)]
      EndpointReferenceType CreateMBean(DynamicMBeanResourceConstructor request);

      [OperationContract(Action = Schema.GetMBeanInfoAction,
         ReplyAction = Schema.GetMBeanInfoResponseAction)]
      ResourceMetaDataType GetMBeanInfo();
   }
}