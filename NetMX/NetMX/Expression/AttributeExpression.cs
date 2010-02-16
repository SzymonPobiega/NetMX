using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   [Serializable]
   public class AttributeExpression : ValueExpression
   {
      private readonly string _attributeName;

      public AttributeExpression(string attributeName)
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

      public override IComparable Evaluate(IQueryEvaluationContext context)
      {
         object value = context.GetAttribute(_attributeName);
         if (value == null)
         {
            throw new InvalidOperationException("Attribute value cannot be null to be used in expression.");
         }
         IComparable result = value as IComparable;
         if (result == null)
         {
            throw new InvalidOperationException("Attribute value must by IComparable to be used in expression.");
         }
         return result;
      }
   }
}