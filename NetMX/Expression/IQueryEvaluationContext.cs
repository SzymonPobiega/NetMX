namespace NetMX
{
   public interface IQueryEvaluationContext
   {
      ObjectName Name { get; }
      object GetAttribute(string attributeName);
      bool HasAttribute(string attributeName);
   }
}