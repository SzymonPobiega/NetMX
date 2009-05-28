using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   public static class IJsr262ServiceContractConstants
   {
      public const string GetDefaultDomainFragmentTransferPath = @"//DefaultDomain/text()";
      public const string GetDomainsFragmentTransferPath = @"//Domain";
   }

   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
   public interface IJsr262ServiceContract
   {
      [OperationContract(Action = WsTransfer.GetAction,
         ReplyAction = WsTransfer.GetResponseAction)]
      [ServiceKnownType(typeof(DynamicMBeanResource))]
      [ServiceKnownType(typeof(GetDefaultDomainResponse))]
      object Get();

      [OperationContract(Action = WsTransfer.PutAction,
         ReplyAction = WsTransfer.PutResponseAction)]
      DynamicMBeanResource SetAttributes(DynamicMBeanResource request);

      [OperationContract(Action = WsTransfer.CreateAction,
         ReplyAction = WsTransfer.CreateResponseAction)]
      EndpointReferenceType CreateMBean(DynamicMBeanResourceConstructor request);

      [OperationContract(Action = WsTransfer.DeleteAction)]
      void UnregisterMBean();

      [OperationContract(Action = Schema.InvokeAction,
         ReplyAction = Schema.InvokeResponseAction)]
      GenericValueType Invoke(OperationRequestType request);      

      [OperationContract(Action = Schema.GetMBeanInfoAction,
         ReplyAction = Schema.GetMBeanInfoResponseAction)]
      ResourceMetaDataType GetMBeanInfo();

      [OperationContract(Action = WsEnumeration.EnumerateAction,
         ReplyAction = WsEnumeration.EnumerateResponseAction)]
      EnumerateResponse Enumerate(Enumerate request);
   }
}