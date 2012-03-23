using System;

namespace NetMX
{
    public class AndExp : BinaryExp<bool, bool, bool>
    {
        public AndExp(IExpression<bool> left, IExpression<bool> right) : base(left, right)
        {
        }

        public override void Accept(IExpressionTreeVisitor visitor)
        {
            base.Accept(visitor);
            visitor.Visit(this);
        }

        public override bool Evaluate(Func<bool> leftValue, Func<bool> rightValue)
        {
            return leftValue() && rightValue();
        }
    }
}