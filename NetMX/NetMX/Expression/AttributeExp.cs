using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetMX
{
   [Serializable]
   public class AttributeExp : QueryExp
   {
      private readonly string _attributeName;

      public AttributeExp(string attributeName)
      {
         if (attributeName == null)
         {
            throw new ArgumentNullException("attributeName", "Attribute name cannot be null.");
         }
         _attributeName = attributeName;
      }

      public string AttributeName
      {
         get { return _attributeName; }
      }

      public override Expression Convert()
      {
         return 
            Expression.Call(Expression.Parameter(typeof(IQueryEvaluationContext), "context"),
                                typeof (IQueryEvaluationContext).GetMethod("GetAttribute"),
                                Expression.Constant(_attributeName));
      }      
   }
}