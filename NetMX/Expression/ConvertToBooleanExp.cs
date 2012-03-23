using System;

namespace NetMX
{
    public class ConvertToBooleanExp : IExpression<bool>
    {
        private readonly IExpression<object> _sourceExpression;

        public ConvertToBooleanExp(IExpression<object> sourceExpression)
        {
            _sourceExpression = sourceExpression;
        }

        public bool Evaluate(IQueryEvaluationContext context)
        {
            return Convert.ToBoolean(_sourceExpression.Evaluate(context));
        }

        public void Accept(IExpressionTreeVisitor visitor)
        {
            _sourceExpression.Accept(visitor);
            visitor.Visit(this);
        }
    }
}