using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class ConstantExp : QueryExp
   {
      private readonly object _constantValue;

      public ConstantExp(object constantValue)
      {
         if (constantValue == null)
         {
            throw new ArgumentNullException("constantValue", "Constant value cannot be null.");
         }
         _constantValue = constantValue;
      }

      public object ConstantValue
      {
         get { return _constantValue; }
      }


      public override Expression Convert()
      {
         return Expression.Constant(_constantValue);
      }
   }
}