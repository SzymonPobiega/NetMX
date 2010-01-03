using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.Transfer
{
   [ServiceContract(Namespace = Const.Namespace)]
   public interface ITransferContract
   {
      [OperationContract(Action = Const.GetAction, ReplyAction = Const.GetResponseAction)]
      Message Get(Message getRequest);

      [OperationContract(Action = Const.PutAction, ReplyAction = Const.PutResponseAction)]
      Message Put(Message putRequest);

      [OperationContract(Action = Const.CreateAction, ReplyAction = Const.CreateResponseAction)]
      Message Create(Message createRequest);

      [OperationContract(Action = Const.DeleteAction, ReplyAction = Const.DeleteResponseAction)]
      Message Delete(Message deleteRequest);
   }
}