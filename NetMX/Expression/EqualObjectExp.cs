using System;

namespace NetMX
{
    public class EqualObjectExp : BinaryExp<bool, object, object>
    {
        public EqualObjectExp(IExpression<object> left, IExpression<object> right)
            : base(left, right)
        {
        }

        public override bool Evaluate(Func<object> leftValue, Func<object> rightValue)
        {
            var left = leftValue();
            var right = rightValue();

            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return true;
            }
            if (ReferenceEquals(null, left) || ReferenceEquals(null, right))
            {
                return false;
            }
            return left.Equals(right);
        }
    }
}