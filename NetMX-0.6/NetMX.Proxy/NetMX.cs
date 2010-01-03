using System;
using System.Collections.Generic;
using System.Text;
using NetMX.Proxy;

namespace NetMX.Proxy
{
   public static class NetMXProxyExtensions
   {
      /// <summary>
      /// Creates new proxy for a MBean in local or remote server.
      /// </summary>
      /// <typeparam name="T">Type of MBean interface.</typeparam>
      /// <param name="connection">Connection to MBean server.</param>
      /// <param name="objectName">ObjectName of MBean to proxy.</param>
      /// <returns>A new proxy instance.</returns>
      public static T NewMBeanProxy<T>(IMBeanServerConnection connection, ObjectName objectName)         
      {
         ProxyInvocationHandler handler = new ProxyInvocationHandler(connection, objectName);
         return (T)ProxyFactory.CreateProxy(typeof(T), handler);
      }
   }
}
