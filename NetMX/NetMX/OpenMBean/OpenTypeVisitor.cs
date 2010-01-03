#region Using
using System;
using System.Globalization;

#endregion

namespace NetMX.OpenMBean
{
   //public delegate void VisitSimpleTypeDelegate(SimpleType visited);
   //public delegate T VisitSimpleTypeDelegate<T>(SimpleType visited);
   //public delegate void VisitArrayTypeDelegate(ArrayType visited);
   //public delegate T VisitArrayTypeDelegate<T>(ArrayType visited);
   //public delegate void VisitTabularTypeDelegate(TabularType visited);
   //public delegate T VisitTabularTypeDelegate<T>(TabularType visited);
   //public delegate void VisitCompositeTypeDelegate(CompositeType visited);
   //public delegate T VisitCompositeTypeDelegate<T>(CompositeType visited);

   //public class DelegatingOpenTypeVisitor : OpenTypeVisitor
   //{
   //   private readonly VisitSimpleTypeDelegate _visitSimple;
   //   private readonly VisitArrayTypeDelegate _visitArray;
   //   private readonly VisitTabularTypeDelegate _visitTabular;
   //   private readonly VisitCompositeTypeDelegate _visitComposite;
   //   private readonly bool _throwOnNotSupported;

   //   public override void VisitSimpleType(SimpleType visited)
   //   {
   //      if (_visitSimple != null)
   //      {
   //         _visitSimple(visited);
   //      }
   //      else if (_throwOnNotSupported)
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitArrayType(ArrayType visited)
   //   {
   //      if (_visitArray != null)
   //      {
   //         _visitArray(visited);
   //      }
   //      else if (_throwOnNotSupported)
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitTabularType(TabularType visited)
   //   {
   //      if (_visitTabular != null)
   //      {
   //         _visitTabular(visited);
   //      }
   //      else if (_throwOnNotSupported)
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitCompositeType(CompositeType visited)
   //   {
   //      if (_visitComposite != null)
   //      {
   //         _visitComposite(visited);
   //      }
   //      else if (_throwOnNotSupported)
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }

   //   public static void VisitOpenType(OpenType openType, bool throwOnNotSupported, VisitSimpleTypeDelegate visitSimple, VisitArrayTypeDelegate visitArray,
   //      VisitTabularTypeDelegate visitTabular, VisitCompositeTypeDelegate visitComposite)
   //   {
   //      OpenTypeVisitor visitor = new DelegatingOpenTypeVisitor(throwOnNotSupported, visitSimple, visitArray, visitTabular, visitComposite);
   //      openType.Visit(visitor);
   //   }

   //   public DelegatingOpenTypeVisitor(bool throwOnNotSupported, VisitSimpleTypeDelegate visitSimple, VisitArrayTypeDelegate visitArray,
   //      VisitTabularTypeDelegate visitTabular, VisitCompositeTypeDelegate visitComposite)
   //   {
   //      _visitSimple = visitSimple;
   //      _visitArray = visitArray;
   //      _visitTabular = visitTabular;
   //      _visitComposite = visitComposite;
   //      _throwOnNotSupported = throwOnNotSupported;
   //   }
   //}

   //public class DelegatingOpenTypeVisitor<T> : OpenTypeVisitor
   //{
   //   private readonly VisitSimpleTypeDelegate<T> _visitSimple;
   //   private readonly VisitArrayTypeDelegate<T> _visitArray;
   //   private readonly VisitTabularTypeDelegate<T> _visitTabular;
   //   private readonly VisitCompositeTypeDelegate<T> _visitComposite;

   //   private T _returnedValue;

   //   public override void VisitSimpleType(SimpleType visited)
   //   {
   //      if (_visitSimple != null)
   //      {
   //         _returnedValue = _visitSimple(visited);
   //      }
   //      else
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitArrayType(ArrayType visited)
   //   {
   //      if (_visitArray != null)
   //      {
   //         _returnedValue = _visitArray(visited);
   //      }
   //      else
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitTabularType(TabularType visited)
   //   {
   //      if (_visitTabular != null)
   //      {
   //         _returnedValue = _visitTabular(visited);
   //      }
   //      else
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }
   //   public override void VisitCompositeType(CompositeType visited)
   //   {
   //      if (_visitComposite != null)
   //      {
   //         _returnedValue = _visitComposite(visited);
   //      }
   //      else
   //      {
   //         throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Open type {0} not supported.", visited));
   //      }
   //   }

   //   public static T VisitOpenType(OpenType openType, VisitSimpleTypeDelegate<T> visitSimple, VisitArrayTypeDelegate<T> visitArray,
   //      VisitTabularTypeDelegate<T> visitTabular, VisitCompositeTypeDelegate<T> visitComposite)
   //   {
   //      DelegatingOpenTypeVisitor<T> visitor = new DelegatingOpenTypeVisitor<T>(visitSimple, visitArray, visitTabular, visitComposite);
   //      openType.Visit(visitor);
   //      return visitor._returnedValue;
   //   }

   //   private DelegatingOpenTypeVisitor(VisitSimpleTypeDelegate<T> visitSimple, VisitArrayTypeDelegate<T> visitArray,
   //      VisitTabularTypeDelegate<T> visitTabular, VisitCompositeTypeDelegate<T> visitComposite)
   //   {
   //      _visitSimple = visitSimple;
   //      _visitArray = visitArray;
   //      _visitTabular = visitTabular;
   //      _visitComposite = visitComposite;         
   //   }
   //}

   //public abstract class OpenTypeVisitor
   //{
   //   public abstract void VisitSimpleType(SimpleType visited);
   //   public abstract void VisitArrayType(ArrayType visited);
   //   public abstract void VisitTabularType(TabularType visited);
   //   public abstract void VisitCompositeType(CompositeType visited);      

   //   protected OpenTypeVisitor()
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