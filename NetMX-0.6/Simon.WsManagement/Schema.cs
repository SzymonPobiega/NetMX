using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Simon.WsManagement
{
   public static class Schema
   {
       public const string AddressingNamespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
       public const string Namespace = "http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd";
       public const string EnumerationNamespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration";
       public const string EventsNamespace = "http://schemas.xmlsoap.org/ws/2004/08/eventing";
   }
}
