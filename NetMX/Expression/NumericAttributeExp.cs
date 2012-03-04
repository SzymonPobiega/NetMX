using System;

namespace NetMX
{
    public class NumericAttributeExp : IExpression<decimal>
    {
        private readonly string _name;

        public NumericAttributeExp(string name)
        {
            _name = name;
        }

        public decimal Evaluate(IQueryEvaluationContext context)
        {
            var value = context.HasAttribute(_name)
                            ? context.GetAttribute(_name)
                            : null;

            if (value == null)
            {
                return 0;
            }
            return Convert.ToDecimal(value);
        }
    }
}