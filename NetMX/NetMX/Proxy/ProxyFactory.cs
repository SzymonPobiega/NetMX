#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NetMX.Configuration.Provider;

#endregion

namespace NetMX.Proxy
{
   /// <summary>
   /// Internal class for constructing proxies using configurable providers.
   /// </summary>
   [ConfigurationSection("netMXProxyFactory", DefaultProvider = true)]
   internal sealed class ProxyFactory : ServiceBase<ProxyProvider>
   {
      private static readonly ProxyFactory _instance = new ProxyFactory();

      internal static object CreateProxy(Type beanInterfaceType, ProxyInvocationHandler handler)
      {
         return _instance.Default.CreateProxy(beanInterfaceType, handler);
      }
   } 
}
