using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simon.WsManagement
{
   public static class WsTransfer
   {
      /// <summary>
      /// WS-Transfer namspace.
      /// </summary>
      public const string Namespace = "http://schemas.xmlsoap.org/ws/2004/09/transfer";
      /// <summary>
      /// URI of WS-Transfer GET action.
      /// </summary>
      public const string GetAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Get";
      /// <summary>
      /// URI of WS-Transfer GET action response.
      /// </summary>
      public const string GetResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/GetResponse";
      /// <summary>
      /// URI of WS-Transfer PUT action.
      /// </summary>
      public const string PutAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Put";
      /// <summary>
      /// URI of WS-Transfer PUT action response.
      /// </summary>
      public const string PutResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/PutResponse";
      /// <summary>
      /// URI of WS-Transfer CREATE action.
      /// </summary>
      public const string CreateAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Create";
      /// <summary>
      /// URI of WS-Transfer CREATE action response.
      /// </summary>
      public const string CreateResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/CreateResponse";
   }
}
