using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class OrExp : QueryExp
   {
      private readonly QueryExp _left;
      private readonly QueryExp _right;

      public OrExp(QueryExp left, QueryExp right)
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

      public override Expression Convert()
      {
         return Expression.Or(_left.Convert(), _right.Convert());
      }
   }
}