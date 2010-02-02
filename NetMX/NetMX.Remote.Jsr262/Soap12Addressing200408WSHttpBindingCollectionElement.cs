using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace NetMX.Remote.Jsr262
{
   public class Soap12Addressing200408WSHttpBindingCollectionElement : StandardBindingCollectionElement<Soap12Addressing200408WSHttpBinding,
                                                                          Soap12Addressing200408WSHttpBindingElement>
   {
      private static readonly MethodInfo _getBindingCollectionElementMethod =
         typeof(Binding).Assembly.GetType("System.ServiceModel.Configuration.ConfigurationHelpers")
            .GetMethod("GetBindingCollectionElement", BindingFlags.NonPublic | BindingFlags.Static);

      internal static Soap12Addressing200408WSHttpBindingCollectionElement GetBindingCollectionElement()
      {         
         return (Soap12Addressing200408WSHttpBindingCollectionElement)
                _getBindingCollectionElementMethod.Invoke(null, new object[] { "soap12Addressing200408WSHttpBinding" });
      }
   }
}