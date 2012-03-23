using System;

namespace NetMX
{
    public class ConvertToNumberExp : IExpression<Number>
    {
        private readonly IExpression<object> _sourceExpression;

        public ConvertToNumberExp(IExpression<object> sourceExpression)
        {
            _sourceExpression = sourceExpression;
        }

        public Number Evaluate(IQueryEvaluationContext context)
        {
            var value = _sourceExpression.Evaluate(context);
            if (value == null)
            {
                return null;
            }
            return Convert.ToDecimal(value);
        }

        public void Accept(IExpressionTreeVisitor visitor)
        {
            _sourceExpression.Accept(visitor);
            visitor.Visit(this);
        }
    }
}