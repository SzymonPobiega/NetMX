using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class NotExp : QueryExp
   {
      private readonly QueryExp _negated;

      public NotExp(QueryExp negated)
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
      
      public override Expression Convert()
      {
         return Expression.Not(_negated.Convert());
      }
   }
}