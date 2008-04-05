#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Simon.Configuration.Provider;
#endregion

namespace NetMX.Proxy
{
   /// <summary>
   /// Abstract class defining provider interface for creating proxies to MBeans.
   /// </summary>
   public abstract class ProxyProvider : ProviderBaseEx
   {
      /// <summary>
      /// Creates proxy object. Proxy implements <paramref name="beanInterfaceType"/> and forwards invocations
      /// to <see cref="NetMX.Proxy.ProxyInvocationHandler"/> instance provided as <paramref name="handler"/>.
      /// </summary>
      /// <param name="beanInterfaceType">Type of MBean interface.</param>
      /// <param name="handler">Handler which invokes MBean server through a connection.</param>
      /// <returns></returns>
      public abstract object CreateProxy(Type beanInterfaceType, ProxyInvocationHandler handler);
   }
}
