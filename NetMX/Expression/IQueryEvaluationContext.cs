using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   public interface IQueryEvaluationContext
   {
      ObjectName Name { get; }
      T GetAttribute<T>(string attributeName);
      bool HasAttribute(string attributeName);
   }
}