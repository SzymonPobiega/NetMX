namespace NetMX
{
    public class OrExp : BinaryExp<bool, bool, bool>
    {
        public OrExp(IExpression<bool> left, IExpression<bool> right)
            : base(left, right)
        {
        }

        public override bool Evaluate(System.Func<bool> leftValue, System.Func<bool> rightValue)
        {
            return leftValue() || rightValue();
        }
    }
}