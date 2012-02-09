using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Enumearation type describing open type value representation.
   /// </summary>
   public enum OpenTypeRepresentation
   {
      /// <summary>
      /// No value.
      /// </summary>
      Void,
      /// <summary>
      /// Boolean value.
      /// </summary>
      Boolean,
      /// <summary>
      /// Character value.
      /// </summary>
      Character,
      /// <summary>
      /// Byte value.
      /// </summary>
      Byte,
      /// <summary>
      /// Short (Int16 value).
      /// </summary>
      Short,
      /// <summary>
      /// Integeer (Int32 value).
      /// </summary>
      Integer,
      /// <summary>
      /// Long (Int64 value).
      /// </summary>
      Long,
      /// <summary>
      /// Float value.
      /// </summary>
      Float,
      /// <summary>
      /// Double precision float value.
      /// </summary>
      Double,
      /// <summary>
      /// String value.
      /// </summary>
      String,
      /// <summary>
      /// Decimal (fixed point) value.
      /// </summary>
      Decimal,
      /// <summary>
      /// Date and time value.
      /// </summary>
      DateTime,
      /// <summary>
      /// Time period value.
      /// </summary>
      TimeSpan,
      /// <summary>
      /// Object name value.
      /// </summary>
      ObjectName,
      /// <summary>
      /// Composite structure value (ICompositeData).
      /// </summary>
      Composite,
      /// <summary>
      /// Tabular value (ITabularData).
      /// </summary>
      Tabular
   }
}