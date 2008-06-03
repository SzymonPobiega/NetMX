using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The TabularType class is the  open type class whose instances describe the types of 
   /// <see cref="ITabularData"/> values. 
   /// </summary>
	[Serializable]   
	public sealed class TabularType : OpenType
   {
      #region Properties
      private readonly ReadOnlyCollection<string> _indexNames;
      /// <summary>
      /// Gets, in the same order as was given to this instance's constructor, an unmodifiable List of 
      /// the names of the items the values of which are used to uniquely index each row element of tabular 
      /// data values described by this TabularType  instance.
      /// </summary>
      public IList<string> IndexNames
      {
         get { return _indexNames; }
      }
      private readonly CompositeType _rowType;
      /// <summary>
      /// Gets the type of the row elements of tabular data values described by this TabularType instance.
      /// </summary>
      public CompositeType RowType
      {
         get { return _rowType; }
      }
      #endregion

      #region Constructor
      /// <summary>
      /// Constructs a TabularType instance, checking for the validity of the given parameters. The validity 
      /// constraints are described below for each parameter.
      /// </summary>
      /// <param name="typeName">The name given to the tabular type this instance represents; cannot be a null or 
      /// empty string.</param>
      /// <param name="description">The human readable description of the tabular type this instance 
      /// represents; cannot be a null or empty string. </param>
      /// <param name="rowType">The type of the row elements of tabular data values described by this tabular 
      /// type instance; cannot be null. </param>
      /// <param name="indexNames">The names of the items the values of which are used to uniquely index each 
      /// row element in the tabular data values described by this tabular type instance; cannot be null or 
      /// empty. Each element should be an item name defined in <paramref name="rowType"/> (no null or empty 
      /// string allowed). It is important to note that the order of the item names in IndexNames is used by 
      /// the methods get and remove of class TabularData to match their array of values parameter to items. </param>
      public TabularType(string typeName, string description, CompositeType rowType, IEnumerable<string> indexNames)
			: base(typeof(ITabularData) , typeName, description)
		{
		   _rowType = rowType;
		   _indexNames = new List<string>(indexNames).AsReadOnly();
      }
      #endregion

      #region Overridden
      public override void Visit(OpenTypeVisitor visitor)
      {
         visitor.VisitTabularType(this);
      }
      public override bool IsValue(object value)
		{
         ITabularData data = value as ITabularData;
         return data != null && data.TabularType.Equals(this);
      }
      public override OpenTypeKind Kind
      {
         get { return OpenTypeKind.TabularType; }
      }
      public override bool Equals(object obj)
      {
         TabularType other = obj as TabularType;         
         if (other != null && TypeName.Equals(other.TypeName) && _rowType.Equals(other._rowType) &&
            _indexNames.Count == other._indexNames.Count)
         {
            for (int i = 0; i < _indexNames.Count; i++ )
            {
               if (_indexNames[i] != other._indexNames[i])
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
      public override int GetHashCode()
      {
         int code = _rowType.GetHashCode() ^ TypeName.GetHashCode();
         foreach (string index in _indexNames)
         {
            code ^= index.GetHashCode();
         }
         return code;
      }
      #endregion
   }
}
