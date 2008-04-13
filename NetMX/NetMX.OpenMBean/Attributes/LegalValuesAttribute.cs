#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   public sealed class LegalValuesAttribute : Attribute
   {
      public object[] _legalValues;
      /// <summary>
      /// Legal values.
      /// </summary>
      public object [] LegalValues
      {
         get { return _legalValues;  }
      }
      /// <summary>
      /// Creates new LegalValuesAttribute object.
      /// </summary>
      /// <param name="legalValues">Legal values.</param>
      public LegalValuesAttribute(object [] legalValues)
      {
         _legalValues = legalValues;
      }
   }
}