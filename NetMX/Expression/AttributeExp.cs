using System;

namespace NetMX
{
    public class AttributeExp<T> : IExpression<T>
    {
        private readonly string _name;

        public AttributeExp(string name)
        {
            _name = name;
        }

        public T Evaluate(IQueryEvaluationContext context)
        {            
            return context.HasAttribute(_name) 
                ? (T)context.GetAttribute(_name) 
                : default(T);
        }
    }
}