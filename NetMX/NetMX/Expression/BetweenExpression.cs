using System;

namespace NetMX
{


   [Serializable]
   public class BetweenExpression : QueryExp
   {
      private readonly ValueExpression _compared;
      private readonly ValueExpression _lowerBound;
      private readonly ValueExpression _upperBound;

      public BetweenExpression(ValueExpression compared, ValueExpression lowerBound, ValueExpression upperBound)
      {
         if (compared == null)
         {
            throw new ArgumentNullException("compared");
         }
         if (lowerBound == null)
         {
            throw new ArgumentNullException("lowerBound");
         }
         if (upperBound == null)
         {
            throw new ArgumentNullException("upperBound");
         }
         _compared = compared;
         _upperBound = upperBound;
         _lowerBound = lowerBound;
      }

      public ValueExpression UpperBound
      {
         get { return _upperBound; }
      }

      public ValueExpression LowerBound
      {
         get { return _lowerBound; }
      }

      public ValueExpression Compared
      {
         get { return _compared; }
      }

      public override bool Match(IQueryEvaluationContext context)
      {
         IComparable comparedValue = _compared.Evaluate(context);
         return comparedValue.CompareTo(_lowerBound.Evaluate(context)) >= 0 &&
                comparedValue.CompareTo(_upperBound.Evaluate(context)) <= 0;
      }
   }
}