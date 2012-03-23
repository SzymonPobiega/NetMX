using System;

namespace NetMX
{
    public class AttributeExp : IExpression<object>
    {
        private readonly IExpression<string > _nameExpression;

        public AttributeExp(string name)
            : this(new ConstantExp<string>(name))
        {
        }

        public AttributeExp(IExpression<string> nameExpression)
        {
            _nameExpression = nameExpression;
        }

        public object Evaluate(IQueryEvaluationContext context)
        {
            var name = _nameExpression.Evaluate(context);
            return context.HasAttribute(name) 
                ? context.GetAttribute(name) 
                : null;
        }

        public void Accept(IExpressionTreeVisitor visitor)
        {
            _nameExpression.Accept(visitor);
            visitor.Visit(this);
        }
    }
}