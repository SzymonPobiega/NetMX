using System;

namespace NetMX
{
    public class ConstantExp<T> : IExpression<T>
    {
        private readonly T _constantValue;

        public ConstantExp(T constantValue)
        {
            _constantValue = constantValue;
        }

        public T Evaluate(IQueryEvaluationContext context)
        {
            return _constantValue;
        }
    }
}