using System;

namespace NetMX
{
    public class GreaterExp : BinaryExp<bool, decimal, decimal>
    {
        public GreaterExp(IExpression<decimal> left, IExpression<decimal> right) 
            : base(left, right)
        {
        }
        public override bool Evaluate(Func<decimal> leftValue, Func<decimal> rightValue)
        {
            return leftValue() > rightValue();
        }
    }
}