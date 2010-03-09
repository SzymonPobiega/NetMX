using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   public class LambdaExp : QueryExp
   {
      private readonly Expression<Func<IQueryEvaluationContext, bool>> _lambdaExpression;

      public LambdaExp(Expression<Func<IQueryEvaluationContext, bool>> lambdaExpression)
      {
         _lambdaExpression = lambdaExpression;
      }

      public Expression<Func<IQueryEvaluationContext, bool>> Lambda
      {
         get { return _lambdaExpression; }
      }

      public override Expression Convert()
      {
         return _lambdaExpression;
      }
   }
}