using System;

namespace NetMX
{
    public abstract class BinaryExp<TResult, TLeft, TRight> : IExpression<TResult>
    {
        private readonly IExpression<TLeft> _left;
        private readonly IExpression<TRight> _right;

        protected BinaryExp(IExpression<TLeft> left, IExpression<TRight> right)
        {
            _left = left;
            _right = right;
        }

        public TResult Evaluate(IQueryEvaluationContext context)
        {
            return Evaluate(() => _left.Evaluate(context), () => _right.Evaluate(context));
        }

        public abstract TResult Evaluate(Func<TLeft> leftValue, Func<TRight> rightValue);
    }
}