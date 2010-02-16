using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   public interface IQueryEvaluationContext
   {
      ObjectName Name { get; }
      string ClassName { get; }
      object GetAttribute(string attributeName);
   }
}