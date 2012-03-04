namespace NetMX
{
    public class HasAttributeExp : IExpression<bool>
    {
        private readonly string _name;

        public HasAttributeExp(string name)
        {
            _name = name;
        }

        public bool Evaluate(IQueryEvaluationContext context)
        {
            return context.HasAttribute(_name);
        }
    }
}