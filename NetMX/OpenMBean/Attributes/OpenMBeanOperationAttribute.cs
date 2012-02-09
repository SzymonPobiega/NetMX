#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public sealed class OpenMBeanOperationAttribute : Attribute
   {
      private readonly OperationImpact _impact;
      /// <summary>
      /// Impact of the operation.
      /// </summary>
      public OperationImpact Impact
      {
         get { return _impact; }
      }
      /// <summary>
      /// Creates new OpenMBeanOperationAttribute object.
      /// </summary>
      /// <param name="impact">Impact of the operation.</param>
      public OpenMBeanOperationAttribute(OperationImpact impact)
      {
         _impact = impact;
      }
   }
}