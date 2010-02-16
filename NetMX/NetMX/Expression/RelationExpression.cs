using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class RelationExpression : QueryExp
   {
      private readonly ValueExpression _left;
      private readonly ValueExpression _right;
      private readonly RelationalOperator _operator;

      public RelationExpression(ValueExpression left, ValueExpression right, RelationalOperator @operator)
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

      public ValueExpression Right
      {
         get { return _right; }
      }

      public ValueExpression Left
      {
         get { return _left; }
      }

      public override bool Match(IQueryEvaluationContext context)
      {
         IComparable left = _left.Evaluate(context);
         if (left == null)
         {
            throw new InvalidOperationException("RelationExpression values cannot be null");
         }
         IComparable right = _right.Evaluate(context);
         if (right == null)
         {
            throw new InvalidOperationException("RelationExpression values cannot be null");
         }
         switch (_operator)
         {
            case RelationalOperator.Eq:
               return left.Equals(right);
            case RelationalOperator.Gt:
               return left.CompareTo(right) > 0;
            case RelationalOperator.Lt:
               return left.CompareTo(right) < 0;
            case RelationalOperator.GtE:
               return left.CompareTo(right) >= 0;
            case RelationalOperator.LtE:
               return left.CompareTo(right) <= 0;
            default:
               throw new NotSupportedException("Not supported operator: "+_operator);
         }
      }
   }
}