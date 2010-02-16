using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class NotExpression : QueryExp
   {
      private readonly QueryExp _negated;

      public NotExpression(QueryExp negated)
      {
         if (negated == null)
         {
            throw new ArgumentNullException("negated");
         }
         _negated = negated;
      }

      public QueryExp Negated
      {
         get { return _negated; }
      }

      public override bool Match(IQueryEvaluationContext instance)
      {
         return !_negated.Match(instance);
      }
   }
}