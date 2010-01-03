#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public sealed class OpenMBeanAttributeAttribute : OpenMBeanAttrParamBase
   {      
      /// <summary>
      /// Creates new OpenMBeanAttributeAttribute object.
      /// </summary>      
      public OpenMBeanAttributeAttribute()
      {         
      }
   }
}