using System;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class BetweenExp : QueryExp
   {
      private readonly QueryExp _compared;
      private readonly QueryExp _lowerBound;
      private readonly QueryExp _upperBound;

      public BetweenExp(QueryExp compared, QueryExp lowerBound, QueryExp upperBound)
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

      public QueryExp UpperBound
      {
         get { return _upperBound; }
      }

      public QueryExp LowerBound
      {
         get { return _lowerBound; }
      }

      public QueryExp Compared
      {
         get { return _compared; }
      }

      public override Expression Convert()
      {
         return Expression.And(
            Expression.GreaterThanOrEqual(_compared.Convert(), _lowerBound.Convert()),
            Expression.LessThanOrEqual(_compared.Convert(), _upperBound.Convert()));

      }
   }
}