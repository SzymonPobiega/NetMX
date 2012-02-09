using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET;
using WSMan.NET.Management;

namespace NetMX.Remote.Jsr262
{
   internal static class ObjectNameSelector
   {
      private const string ObjectName = "ObjectName";

      internal static IEnumerable<Selector> CreateSelectorSet(this ObjectName objectName)
      {
         if (objectName == null)
         {
            return new Selector[] {};
         }
         return new [] {new Selector(ObjectName, objectName)};
      }

      internal static ObjectName ExtractObjectName(this EndpointAddress address)
      {
         return SelectorSetHeader.ReadFrom(address).ExtractObjectName();
      }

      internal static ObjectName ExtractObjectName(this SelectorSetHeader selectors)
      {
         foreach (Selector selector in selectors.Selectors)
         {
            if (selector.Name == ObjectName)
            {
               return selector.SimpleValue;
            }
         }
         throw new InvalidOperationException();
      }

      internal static EndpointReference ToEndpointReference(this ObjectName name)
      {
         return new EndpointReference(CreateEndpointAddress(name));
      }

      internal static EndpointAddress CreateEndpointAddress(ObjectName name)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder
         {
            Uri =
               OperationContext.Current.Channel != null
                  ? OperationContext.Current.Channel.LocalAddress.Uri
                  : OperationContext.Current.Extensions.Find<ServerAddressExtension>
                       ().Address
         };
         builder.Headers.Add(CreateSelectorSetHeader(name));
         return builder.ToEndpointAddress();
      }      

      internal static ObjectName ExtractObjectName(this IEnumerable<Selector> selectors)
      {
         foreach (Selector selector in selectors)
         {
            if (selector.Name == ObjectName)
            {
               return selector.SimpleValue;
            }
         }
         return null;
      }

      public static SelectorSetHeader CreateSelectorSetHeader(ObjectName name)
      {
         return new SelectorSetHeader(CreateSelectorSet(name));
      }
   }
}
