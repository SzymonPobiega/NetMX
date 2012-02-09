using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class ArithmeticExp : QueryExp
   {
      private readonly QueryExp _left;
      private readonly QueryExp _right;
      private readonly ArithmeticOperator _operator;

      public ArithmeticExp(QueryExp left, QueryExp right, ArithmeticOperator @operator)
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
         switch (Operator)
         {
            case ArithmeticOperator.Add:
               return Expression.Add(left, right);
            case ArithmeticOperator.Sub:
               return Expression.Subtract(left, right);
            case ArithmeticOperator.Mul:
               return Expression.Multiply(left, right);
            case ArithmeticOperator.Div:
               return Expression.Divide(left, right);
            default:
               throw new NotSupportedException("Operator not supported: " + Operator);
         }
      }
   }
}