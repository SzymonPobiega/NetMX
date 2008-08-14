using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.Monitor
{
   internal interface INumericUtil
   {
      IComparable Add(object first, object second);
      IComparable Sub(object first, object second);
      IComparable Zero { get; }
   }
   internal static class NumericUtils
   {
      private static Dictionary<Type, INumericUtil> _utils = new Dictionary<Type, INumericUtil>();

      internal static INumericUtil GetUtil(Type valueType)
      {
         return null;
      }           
   }
}
