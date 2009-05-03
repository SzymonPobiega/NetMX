using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Simon.WsManagement;

namespace NetMX.Remote.Jsr262
{
   internal static class ObjectNameSelector
   {
      private const string ObjectName = "ObjectName";

      internal static SelectorSetHeader CreateSelectorSet(ObjectName objectName)
      {
         return new SelectorSetHeader(new Selector(ObjectName, objectName));
      }
      internal static ObjectName ExtractObjectName(this EndpointAddress address)
      {
         return null;
      }

      internal static ObjectName ExtractObjectName(this SelectorSetHeader header)
      {
         foreach (Selector selector in header.Selectors)
         {
            if (selector.Name == ObjectName)
            {
               return selector.SimpleValue;
            }
         }
         throw new InvalidOperationException();
      }
   }
}
