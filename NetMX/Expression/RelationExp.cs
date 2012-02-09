using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class RelationExp : QueryExp
   {
      private readonly QueryExp _left;
      private readonly QueryExp _right;
      private readonly RelationalOperator _operator;

      public RelationExp(QueryExp left, QueryExp right, RelationalOperator @operator)
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
         _operator = @operator;
         _right = right;
      }

      public RelationalOperator Operator
      {
         get { return _operator; }
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
         Expression left = _left.Convert();
         Expression right = _right.Convert();
         switch (_operator)
         {
            case RelationalOperator.Eq:
               return Expression.Equal(left, right);
            case RelationalOperator.Gt:
               return Expression.GreaterThan(left, right);
            case RelationalOperator.Lt:
               return Expression.LessThan(left, right);
            case RelationalOperator.GtE:
               return Expression.GreaterThan(left, right);
            case RelationalOperator.LtE:
               return Expression.LessThanOrEqual(left, right);
            default:
               throw new NotSupportedException("Not supported operator: " + _operator);
         }
      }
   }
}