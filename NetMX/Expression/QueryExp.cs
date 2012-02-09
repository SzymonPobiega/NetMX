using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetMX
{
   [Serializable]
	public abstract class QueryExp
	{
      public abstract Expression Convert();

      private static readonly Dictionary<QueryExp, Func<IQueryEvaluationContext, bool>> _cache = new Dictionary<QueryExp, Func<IQueryEvaluationContext, bool>>();
      
      public static Func<IQueryEvaluationContext, bool> Translate(QueryExp exp)
      {
         lock (_cache)
         {
            Func<IQueryEvaluationContext, bool> existing;
            if (!_cache.TryGetValue(exp, out existing))
            {
               existing = Compile(exp);
               _cache[exp] = existing;
            }
            return existing;
         }
      }

      private static Func<IQueryEvaluationContext, bool> Compile(QueryExp exp)
      {
         LambdaExp lambdaExp = exp as LambdaExp;
         if (lambdaExp != null)
         {
            return lambdaExp.Lambda.Compile();
         }
         return Expression.Lambda<Func<IQueryEvaluationContext, bool>>(
            exp.Convert(), Expression.Parameter(typeof(IQueryEvaluationContext), "context")).Compile();
      } 
	}
}
