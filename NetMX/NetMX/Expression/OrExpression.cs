using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class OrExpression : QueryExp
   {
      private readonly QueryExp _left;
      private readonly QueryExp _right;

      public OrExpression(QueryExp left, QueryExp right)
      {
         if (left == null)
         {
            throw new ArgumentNullException("left");
         }
         if (right == null)
         {
            throw new ArgumentNullException("right");
         }
         _left = left;
         _right = right;
      }

      public QueryExp Right
      {
         get { return _right; }
      }

      public QueryExp Left
      {
         get { return _left; }
      }

      public override bool Match(IQueryEvaluationContext instance)
      {
         return _left.Match(instance) || _right.Match(instance);
      }
   }
}