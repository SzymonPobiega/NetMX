using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Simon.WsManagement
{
   [MessageContract]
   public sealed class ResourceCreated
   {
      private EndpointAddress _epa;

      [MessageBodyMember(Name = WsTransfer.CreateAction, Namespace = WsTransfer.Namespace, Order = 0)]
      public EndpointAddress10 EndpointAddress10
      {
         get { return EndpointAddress10.FromEndpointAddress(_epa); }
         set { _epa = value.ToEndpointAddress(); }
      }
      public EndpointAddress EndpointAddress
      {
         get { return _epa; }
         set { _epa = value; }
      }

      public ResourceCreated()
      {         
      }

      public ResourceCreated(EndpointAddress epa)
      {
         _epa = epa;
      }
   }
}
