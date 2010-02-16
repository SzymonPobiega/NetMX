using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class ArithmeticExpression : ValueExpression
   {
      private readonly ValueExpression _left;
      private readonly ValueExpression _right;
      private readonly ArithmeticOperator _operator;

      public ArithmeticExpression(ValueExpression left, ValueExpression right, ArithmeticOperator @operator)
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

      public ArithmeticOperator Operator
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

      public override IComparable Evaluate(IQueryEvaluationContext context)
      {
         IComparable left = _left.Evaluate(context);
         if (left == null)
         {
            throw new InvalidOperationException("ArithmeticExpression values cannot be null");
         }
         IComparable right = _right.Evaluate(context);
         if (right == null)
         {
            throw new InvalidOperationException("ArithmeticExpression values cannot be null");
         }
         switch (_operator)
         {
            default:
               throw new NotSupportedException("Not supported operator: "+_operator);
         }
      }
   }
}