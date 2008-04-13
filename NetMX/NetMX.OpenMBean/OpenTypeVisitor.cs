#region Using
using System;

#endregion

namespace NetMX.OpenMBean
{
   //public abstract class OpenTypeVisitor
   //{
   //   public abstract void VisitSimpleType(SimpleType visited);
   //   public abstract void VisitArrayType(ArrayType visited);
   //   public abstract void VisitTabularType(TabularType visited);
   //   public abstract void VisitCompositeType(CompositeType visited);

   //   internal void VisitSimpleTypeInternal(SimpleType visited)
   //   {
         
   //   }
   //}

   public enum OpenTypeKind
   {
      /// <summary>
      /// Represents a simple type.
      /// </summary>
      SimpleType,
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