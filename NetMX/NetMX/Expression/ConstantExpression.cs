using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class ConstantExpression : ValueExpression
   {
      private readonly IComparable _constantValue;

      public ConstantExpression(IComparable constantValue)
      {
         if (constantValue == null)
         {
            throw new ArgumentNullException("constantValue", "Constant value cannot be null.");
         }
         _constantValue = constantValue;
      }

      public IComparable ConstantValue
      {
         get { return _constantValue; }
      }

      public override IComparable Evaluate(IQueryEvaluationContext context)
      {
         return _constantValue;
      }
   }
}