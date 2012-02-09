using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

namespace NetMX.Proxy.Castle
{
   public class CastleProxyProvider : ProxyProvider
   {
      private ProxyGenerator _generator = new ProxyGenerator();

      public override object CreateProxy(Type beanInterfaceType, ProxyInvocationHandler handler)
      {
         return _generator.CreateProxy(beanInterfaceType, new Interceptor(handler), new object());
      }

      private class Interceptor : IInterceptor
      {
         private ProxyInvocationHandler _handler;

         public Interceptor(ProxyInvocationHandler handler)
         {
            _handler = handler;
         }

         #region IInterceptor Members
         public object Intercept(IInvocation invocation, params object[] args)
         {
            return _handler.HandleInvocation(invocation.Method, args);
         }
         #endregion
      }
   }
}
