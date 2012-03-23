using System;

namespace NetMX
{
    public class EqualExp : BinaryExp<bool, object, object>
    {
        public EqualExp(IExpression<object> left, IExpression<object> right)
            : base(left, right)
        {
        }

        public override void Accept(IExpressionTreeVisitor visitor)
        {
            base.Accept(visitor);
            visitor.Visit(this);
        }

        public override bool Evaluate(Func<object> leftValue, Func<object> rightValue)
        {
            var left = leftValue();
            var right = rightValue();
            
            if (ReferenceEquals(null, left) || ReferenceEquals(null, right))
            {
                return false;
            }
            return left.Equals(right);
        }
    }
}