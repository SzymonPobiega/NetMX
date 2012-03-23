using System;

namespace NetMX
{
    public class HasAttributeExp : IExpression<bool>
    {
        private readonly IExpression<string > _nameExpression;

        public HasAttributeExp(string name)
            : this(new ConstantExp<string>(name))
        {
        }

        public HasAttributeExp(IExpression<string> nameExpression)
        {
            _nameExpression = nameExpression;
        }

        public bool Evaluate(IQueryEvaluationContext context)
        {
            var name = _nameExpression.Evaluate(context);
            return context.HasAttribute(name);
        }

        public void Accept(IExpressionTreeVisitor visitor)
        {
            _nameExpression.Accept(visitor);
            visitor.Visit(this);
        }
    }
}