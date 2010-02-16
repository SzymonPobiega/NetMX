using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class InExpression : QueryExp
   {
      private readonly ValueExpression _compared;
      private readonly ValueExpression[] _allowedValues;

      public InExpression(ValueExpression compared, IEnumerable<ValueExpression> allowedValues)
      {
         _compared = compared;
         _allowedValues = allowedValues.ToArray();
      }

      public ValueExpression[] AllowedValues
      {
         get { return _allowedValues; }
      }

      public ValueExpression Compared
      {
         get { return _compared; }
      }

      public override bool Match(IQueryEvaluationContext context)
      {
         object comparedValue = _compared.Evaluate(context);
         return _allowedValues.Any(x => x.Evaluate(context) == comparedValue);
      }
   }
}