using System;
using System.Linq;
using System.Collections.Generic;

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

      public T GetAttribute<T>(string attributeName)
      {
         return (T)_bean.GetAttribute(attributeName);
      }

      public bool HasAttribute(string attributeName)
      {
         return _bean.GetMBeanInfo().Attributes.Any(x => x.Name == attributeName);
      }
   }
}