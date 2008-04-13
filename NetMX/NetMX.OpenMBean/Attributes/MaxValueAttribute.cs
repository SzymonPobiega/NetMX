#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   public sealed class MaxValueAttribute : Attribute
   {
      private readonly object _value;
      /// <summary>
      /// Maximum value.
      /// </summary>
      public object Value
      {
         get { return _value; }
      }
      /// <summary>
      /// Creates new MaxValueAttribute object.
      /// </summary>
      /// <param name="value">Maximum value.</param>
      public MaxValueAttribute(object value)
      {
         _value = value;
      }
   }
}