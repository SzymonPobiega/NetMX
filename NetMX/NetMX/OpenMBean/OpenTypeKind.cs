#region Using
using System;
using System.Globalization;

#endregion

namespace NetMX.OpenMBean
{   
   public enum OpenTypeKind
   {
      /// <summary>
      /// Represents a simple type.
      /// </summary>
      SimpleType,
      /// <summary>
      /// Represents an enumeration type.
      /// </summary>
      EnumerationType,
      /// <summary>
      /// Represents an array type.
      /// </summary>
      ArrayType,
      /// <summary>
      /// Represents a tabular type.
      /// </summary>
      TabularType,
      /// <summary>
      /// Represents a composite type.
      /// </summary>
      CompositeType
   }
}