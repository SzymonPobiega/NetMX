using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

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
      [OperationContract(Action = Schema.InvokeAction, ReplyAction = Schema.InvokeResponseAction)]
      InvokeResponseMessage Invoke(InvokeMessage request);

      [OperationContract(Action = Schema.GetMBeanInfoAction, ReplyAction = Schema.GetMBeanInfoResponseAction)]
      ResourceMetaDataTypeMessage GetMBeanInfo();
      
      [OperationContract(Action = Schema.InstanceOfAction, ReplyAction = Schema.InstanceOfResponseAction)]
      IsInstanceOfResponseMessage IsInstanceOf(IsInstanceOfMessage className);      
   }
}