using System.Linq;

namespace NetMX.Server
{
   public class EvaluationContext : IQueryEvaluationContext
   {
      private readonly ObjectName _name;
      private readonly IDynamicMBean _bean;

      public EvaluationContext(ObjectName name, IDynamicMBean bean)
      {
         _name = name;
         _bean = bean;
      }

      public ObjectName Name
      {
         get { return _name; }
      }
      
      public string ClassName
      {
         get { return _bean.GetMBeanInfo().ClassName; }
      }

      public object GetAttribute(string attributeName)
      {
         return _bean.GetAttribute(attributeName);
      }

      public bool HasAttribute(string attributeName)
      {
         return _bean.GetMBeanInfo().Attributes.Any(x => x.Name == attributeName);
      }
   }
}