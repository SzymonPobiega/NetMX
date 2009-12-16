using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   public static class IJsr262ServiceContractConstants
   {
      public const string GetDefaultDomainFragmentTransferPath = @"//jmx:DefaultDomain/text()";
      public const string GetDomainsFragmentTransferPath = @"//jmx:Domain";
   }

   [ServiceContract, FragmentHeader]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
   public interface IJsr262ServiceContract : IDisposable
   {
      [OperationContract(Action = WsTransfer.GetAction, ReplyAction = WsTransfer.GetResponseAction)]
      [ServiceKnownType(typeof(GetDefaultDomainResponse))]
      [ServiceKnownType(typeof(DynamicMBeanResource))]
      [ServiceKnownType(typeof(GetDomainsResponse))]      
      GetResponseMessage Get();

      [OperationContract(Action = WsTransfer.PutAction, ReplyAction = WsTransfer.PutResponseAction)]
      [ServiceKnownType(typeof(DynamicMBeanResource))]
      SetAttributesResponseMessage SetAttributes(SetAttributesMessage request);

      [OperationContract(Action = WsTransfer.CreateAction, ReplyAction = WsTransfer.CreateResponseAction)]
      EndpointReferenceType CreateMBean(DynamicMBeanResourceConstructor request);

      [OperationContract(Action = WsTransfer.DeleteAction)]
      void UnregisterMBean();

      [OperationContract(Action = Schema.InvokeAction, ReplyAction = Schema.InvokeResponseAction)]
      InvokeResponseMessage Invoke(InvokeMessage request);

      [OperationContract(Action = Schema.GetMBeanInfoAction, ReplyAction = Schema.GetMBeanInfoResponseAction)]
      ResourceMetaDataTypeMessage GetMBeanInfo();

      [OperationContract(Action = WsEnumeration.EnumerateAction, ReplyAction = WsEnumeration.EnumerateResponseAction)]      
      EnumerateResponseMessage Enumerate(Message request);

      [OperationContract(Action = Schema.InstanceOfAction, ReplyAction = Schema.InstanceOfResponseAction)]
      IsInstanceOfResponseMessage IsInstanceOf(IsInstanceOfMessage className);

      [OperationContract(Action = Schema.SubscribeAction, ReplyAction = Schema.SubscribeResponseAction)]
      Message Subscribe(Message msg);

      [OperationContract(Action = Schema.UnsubscribeAction, ReplyAction = Schema.UnsubscribeResponseAction)]
      void Unsubscribe(Message msg);

      [OperationContract(Action = WsEnumeration.PullAction, ReplyAction = WsEnumeration.PullResponseAction)]      
      PullResponseMessage Pull(Message request);
   }
}