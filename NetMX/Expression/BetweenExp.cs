using System;

namespace NetMX
{
    public class BetweenExp : IExpression<bool>
    {
        private readonly IExpression<bool > _expression;
        
        public BetweenExp(IExpression<decimal> input, IExpression<decimal> lowerBound, bool lowerInclusive, IExpression<decimal> upperBound, bool upperInclusive)
        {
            _expression = new AndExp(CreateExpression(input, lowerBound, lowerInclusive), CreateExpression(upperBound, input, upperInclusive));
        }

        private static IExpression<bool> CreateExpression(IExpression<decimal> left, IExpression<decimal> right, bool inclusive)
        {
            if (inclusive)
            {
                return new GreaterOrEqualExp(left, right);
            }
            return new GreaterExp(left, right);
        }

        public bool Evaluate(IQueryEvaluationContext context)
        {
            return _expression.Evaluate(context);
        }
    }
}