namespace NetMX
{
    public class AddExp : BinaryExp<decimal, decimal, decimal>
    {
        public AddExp(IExpression<decimal> left, IExpression<decimal> right) : base(left, right)
        {
        }

        public override decimal Evaluate(System.Func<decimal> leftValue, System.Func<decimal> rightValue)
        {
            return leftValue() + rightValue();
        }
    }
}