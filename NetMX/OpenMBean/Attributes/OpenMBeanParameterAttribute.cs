#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
   public sealed class OpenMBeanParameterAttribute : OpenMBeanAttrParamBase
   {      
      /// <summary>
      /// Creates new OpenMBeanParameterAttribute object.
      /// </summary>      
      public OpenMBeanParameterAttribute()
      {                  
      }
   }
}