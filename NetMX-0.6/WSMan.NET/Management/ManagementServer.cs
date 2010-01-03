using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Transfer;

namespace WSMan.NET.Management
{
   public class ManagementServer : ITransferRequestHandler
   {
      private readonly List<IManagementRequestHandler> _handlers;

      public ManagementServer(params IManagementRequestHandler[] handlers)
      {
         _handlers = handlers.ToList();
      }

      public ManagementServer(List<IManagementRequestHandler> handlers)
      {
         _handlers = handlers;
      }     

      public object HandleGet()
      {
         FragmentTransferHeader fragmentTransferHeader =
            FragmentTransferHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);         

         OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);

         return GetHandler().HandleGet(fragmentTransferHeader.Expression, GetSelectors());
      }     

      public object HandlePut(ExtractBodyDelegate extractBodyCallback)
      {
         FragmentTransferHeader fragmentTransferHeader =
            FragmentTransferHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         
         OperationContext.Current.OutgoingMessageHeaders.Add(fragmentTransferHeader);

         return GetHandler().HandlePut(fragmentTransferHeader.Expression, GetSelectors(), extractBodyCallback);
      }

      public EndpointAddress HandleCreate(ExtractBodyDelegate extractBodyCallback)
      {
         return GetHandler().HandleCreate(extractBodyCallback);
      }

      public void HandlerDelete()
      {         
         GetHandler().HandlerDelete(GetSelectors());
      }

      private IManagementRequestHandler GetHandler()
      {
         ResourceUriHeader resourceUriHeader =
            ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         return _handlers.First(x => x.CanHandle(resourceUriHeader.ResourceUri));
      }


      private static List<Selector> GetSelectors()
      {
         SelectorSetHeader selectorSetHeader =
            SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);

         List<Selector> selectors = selectorSetHeader != null 
            ? selectorSetHeader.Selectors 
            : new List<Selector>();
         return selectors;
      }
   }
}