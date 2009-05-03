using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace Simon.WsManagement
{
   [XmlRoot("ResourceCreated", Namespace = WsTransfer.Namespace)]
   public sealed class ResourceCreated
   {
      private EndpointAddress _epa;

      [XmlElement(ElementName = "" )]
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
