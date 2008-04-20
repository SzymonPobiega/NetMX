using System;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The ArrayType class is the open type class whose instances describe all open data values which 
   /// are n-dimensional arrays of open data values.
   /// </summary>
   [Serializable]   
   public sealed class ArrayType : OpenType
   {
      #region Properties
      private readonly int _dimension;
      /// <summary>
      /// Gets the dimension of arrays described by this ArrayType instance.
      /// </summary>
      public int Dimension
      {
         get { return _dimension; }
      }
      private readonly OpenType _elementType;
      /// <summary>
      /// Gets the open type of element values contained in the arrays described by this ArrayType instance.
      /// </summary>
      public OpenType ElementType
      {
         get { return _elementType; }
      }
      #endregion

      #region Constructor
      /// <summary>
      /// Constructs an ArrayType instance describing open data values which are arrays with dimension dimension 
      /// of elements whose open type is elementType.
      /// </summary>
      /// <param name="dimension">the dimension of arrays described by this ArrayType instance; must be 
      /// greater than or equal to 1.</param>
      /// <param name="elementType">the open type of element values contained in the arrays described by this 
      /// ArrayType instance; must be an instance of either <see cref="SimpleType"/>, <see cref="CompositeType"/> or <see cref="TabularType"/>.</param>
      /// <exception cref="OpenDataException">if elementType is an instance of ArrayType</exception>
      public ArrayType(int dimension, OpenType elementType)
         : base(elementType.Representation.MakeArrayType(dimension),
         elementType.Representation.MakeArrayType(dimension).FullName,
         string.Format("{0}-dimension array of {1}", dimension, 
         elementType.Representation.MakeArrayType(dimension).FullName))
      {
         if (elementType is ArrayType)
         {
            throw new OpenDataException("Element type cannot be an instance of ArrayType.");
         }
         _dimension = dimension;
         _elementType = elementType;
      }
      #endregion

      #region Overridden
      public override bool Equals(object obj)
      {
         ArrayType other = obj as ArrayType;
         return other != null && _dimension == other._dimension && _elementType.Equals(other._elementType);
      }
      public override int GetHashCode()
      {
         return _dimension.GetHashCode() ^ _elementType.GetHashCode();
      }
      public override void Visit(OpenTypeVisitor visitor)
      {
         visitor.VisitArrayType(this);
      }
      public override bool IsValue(object value)
      {
         Array array = value as Array;
         if (array != null)
         {
            if (array.Rank != _dimension)
            {
               return false;
            }
            int count = 1;
            int[] index = null;
            int[] lengths = new int[array.Rank];
            int[] lowerBounds = new int[array.Rank];
            for (int i = 0; i < array.Rank; i++)
            {
               lengths[i] = array.GetLength(i);
               count *= lengths[i];
               lowerBounds[i] = array.GetLowerBound(i);
            }            
            for (int i = 0; i < count; i++)
            {
               index = GetNextIndex(index, lengths, lowerBounds);
               object element = array.GetValue(index);
               if (element != null && !ElementType.IsValue(element))
               {
                  return false;
               }
            }
            return true;
         }
         else
         {
            return false;
         }
      }
      public override OpenTypeKind Kind
      {
         get { return OpenTypeKind.ArrayType; }
      }
      #endregion

      #region Utility
      private static int[] GetNextIndex(int[] prevIndex, int[] lengths, int[] lowerBounds)
      {
         if (prevIndex == null)
         {
            int[] result = new int[lengths.Length];
            for( int i = 0; i < result.Length; i++)
            {
               result[i] = lowerBounds[i];
            }
            return result;
         }
         else
         {
            int i = prevIndex.Length - 1;
            while ((prevIndex[i] == lengths[i] - 1) && i >= 0)
            {
               i--;
            }
            if (i < 0)
            {
               return null;
            }
            prevIndex[i]++;
            for (int j = i + 1; j < prevIndex.Length; j++)
            {
               prevIndex[j] = lowerBounds[j];
            }
         }
         return prevIndex;
      }
      #endregion
   }
}