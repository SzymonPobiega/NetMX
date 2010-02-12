using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean
{   
	/// <summary>
	/// The OpenType class is the parent abstract class of all classes which describe the actual open type of open 
	/// data values.
	/// An open type is defined by:
	/// <list type="bullet">
	/// <item>the fully qualified class name of the open data values this type describes; note that only a limited
   /// set of classes is allowed for open data values (see ALLOWED_CLASSNAMES),</item>
	/// <item>its name,</item>
	/// <item>its description.</item>
	/// </list>
	/// </summary>
	[Serializable]
	public abstract class OpenType
   {
      #region Properties
      private readonly string _name;
		/// <summary>
		/// Gets the name of this OpenType instance.
		/// </summary>
		public string TypeName
		{
			get { return _name; }
		}
		private readonly string _description;
		/// <summary>
		/// Gets the text description of this OpenType instance.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}      
		private readonly string _representationTypeName;
		/// <summary>
		/// Gets the value representation (physical) of this open type.
		/// </summary>
		public Type Representation
		{
			get { return Type.GetType(_representationTypeName, true); }
		}
		#endregion

		#region Constructor
		protected OpenType(Type representation, string typeName, string description)
		{
			_representationTypeName = representation.AssemblyQualifiedName;
			_name = typeName;
			_description = description;
		}
		#endregion

		#region Abstract
		public abstract bool IsValue(object value);
      public abstract OpenTypeKind Kind { get; }	   
		#endregion

      #region Operator
      public static bool operator == (OpenType left, OpenType right)
      {
         return (ReferenceEquals(left, null) && ReferenceEquals(right, null)) ||
                (!ReferenceEquals(left, null) && !ReferenceEquals(right, null) &&
                 left.Equals(right));
      }
	   public static bool operator !=(OpenType left, OpenType right)
	   {
	      return !(left == right);
	   }
	   #endregion

      #region Factory
      public static OpenType CreateOpenType(Type type)
      {
         if (type.IsArray)
         {
            return new ArrayType(type.GetArrayRank(), CreateOpenType(type.GetElementType()));
         }
         if (SimpleType.IsSimpleType(type))
         {
            return SimpleType.CreateSimpleType(type);
         }
         throw new NotSupportedException("Not supported type.");
      }      
      #endregion
   }
}
