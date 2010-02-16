using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public abstract class ValueExpression
   {
      public abstract IComparable Evaluate(IQueryEvaluationContext context);
   }
}