using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simon.WsManagement
{
   public static class WsEnumeration
   {      
      /// <summary>
      /// WS-Enumeration enumerate action.
      /// </summary>
      public const string EnumerateAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Enumerate";
      /// <summary>
      /// WS-Enumeration enumerate response action.
      /// </summary>
      public const string EnumerateResponseAction =
         @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/EnumerateResponse";

      /// <summary>
      /// WS-Enumeration enumerate action.
      /// </summary>
      public const string PullAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Pull";

       /// <summary>
       /// WS-Enumeration enumerate response action.
       /// </summary>
       public const string PullResponseAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/PullResponse";

   }
}
