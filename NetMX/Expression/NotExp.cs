using System;

namespace NetMX
{
    public class NotExp : UnaryExp<bool, bool>
    {
        public NotExp(IExpression<bool> input) : base(input)
        {
        }

        public override bool Evaluate(Func<bool> inputValue)
        {
            return !inputValue();
        }
    }
}