using System;

namespace NetMX
{
    public class LessExp : BinaryExp<bool, decimal, decimal>
    {
        public LessExp(IExpression<decimal> left, IExpression<decimal> right)
            : base(left, right)
        {
        }
        public override bool Evaluate(Func<decimal> leftValue, Func<decimal> rightValue)
        {
            return leftValue() < rightValue();
        }
    }
}