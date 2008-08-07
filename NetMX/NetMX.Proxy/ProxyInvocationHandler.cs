#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#endregion

namespace NetMX.Proxy
{
   /// <summary>
   /// Internal class for handling intercepted invocations. It forwards the invocation to local or remote
   /// MBean server based on following policy:
   /// <list type="bullet">
   /// <item>If method has "special name" and its name starts with "get_", it is forwarded as <see cref="NetMX.IMBeanServerConnection.GetAttribute"/>() method.</item>
   /// <item>If method has "special name" and its name starts with "set_", it is forwarded as <see cref="NetMX.IMBeanServerConnection.SetAttribute"/>() method.</item>
   /// <item>If method doesn't have "special name"as <see cref="NetMX.IMBeanServerConnection.Invoke"/>() method.</item>
   /// </list>
   /// </summary>
   public sealed class ProxyInvocationHandler
   {
      private IMBeanServerConnection _connection;
      private ObjectName _name;

      internal ProxyInvocationHandler(IMBeanServerConnection connection, ObjectName objectName)
      {
         _connection = connection;
         _name = objectName;
      }

      public object HandleInvocation(MethodBase targetMethod, object[] arguments)
      {
         if (targetMethod.IsSpecialName)
         {
            if (targetMethod.Name.StartsWith("get_"))
            {
               return _connection.GetAttribute(_name, targetMethod.Name.Replace("get_", ""));
            }
            else if (targetMethod.Name.StartsWith("set_"))
            {
               _connection.SetAttribute(_name, targetMethod.Name.Replace("set_", ""), arguments[0]);
               return null;
            }
            else
            {
               throw new NotSupportedException("Not supported special-name method type: "+targetMethod.Name);
            }
         }
         else
         {
            return _connection.Invoke(_name, targetMethod.Name, arguments);
         }
      }
   }
}
