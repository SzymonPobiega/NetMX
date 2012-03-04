namespace NetMX
{
    public class AndExp : BinaryExp<bool, bool, bool>
    {
        public AndExp(IExpression<bool> left, IExpression<bool> right) : base(left, right)
        {
        }

        public override bool Evaluate(System.Func<bool> leftValue, System.Func<bool> rightValue)
        {
            return leftValue() && rightValue();
        }
    }
}