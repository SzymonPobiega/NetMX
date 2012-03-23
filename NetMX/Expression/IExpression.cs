namespace NetMX
{
    public interface IExpression
    {
        void Accept(IExpressionTreeVisitor visitor);
    }

    public interface IExpression<out T> : IExpression
    {
        T Evaluate(IQueryEvaluationContext context);
    }
}