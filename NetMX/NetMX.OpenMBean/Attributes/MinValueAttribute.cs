#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   public sealed class MinValueAttribute : Attribute
   {
      private readonly object _value;
      /// <summary>
      /// Minimum value.
      /// </summary>
      public object Value
      {
         get { return _value; }  
      }
      /// <summary>
      /// Creates new MinValueAttribute object.
      /// </summary>
      /// <param name="value">Minimum value.</param>
      public MinValueAttribute(object value)
      {         
         _value = value;
      }
   }
}