using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   public static class ExpressionUtils
   {
      public static Expression ConvertToDecimal(Expression expression)
      {
         return Expression.Call(typeof (Convert).GetMethod("ToDecimal", new[] {typeof (object)}), expression); 
      }
   }
}