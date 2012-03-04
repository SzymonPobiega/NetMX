namespace NetMX
{
    public interface IExpression<out T>
    {
        T Evaluate(IQueryEvaluationContext context);
    }
}