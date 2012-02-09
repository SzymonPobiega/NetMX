using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class InExp : QueryExp
   {
      private readonly QueryExp _compared;
      private readonly QueryExp[] _allowedValues;

      public InExp(QueryExp compared, IEnumerable<QueryExp> allowedValues)
      {
         _compared = compared;
         _allowedValues = allowedValues.ToArray();
      }

      public QueryExp[] AllowedValues
      {
         get { return _allowedValues; }
      }

      public QueryExp Compared
      {
         get { return _compared; }
      }

      //public override bool Match(IQueryEvaluationContext context)
      //{
      //   object comparedValue = _compared.Evaluate(context);
      //   return _allowedValues.Any(x => x.Evaluate(context) == comparedValue);
      //}

      public override Expression Convert()
      {
         throw new NotImplementedException();
      }
   }
}