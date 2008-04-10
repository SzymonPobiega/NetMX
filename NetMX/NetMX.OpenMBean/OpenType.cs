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
	/// <item>the fully qualified class name of the open data values this type describes; note that only a limited</item>
	/// set of classes is allowed for open data values (see ALLOWED_CLASSNAMES),
	/// <item>its name,</item>
	/// <item>its description.</item>
	/// </list>
	/// </summary>
	public abstract class OpenType
	{
		#region Properties
		private string _name;
		/// <summary>
		/// Gets the name of this OpenType instance.
		/// </summary>
		public string TypeName
		{
			get { return _name; }
		}
		private string _description;
		/// <summary>
		/// Gets the text description of this OpenType instance.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}
		private OpenTypeRepresentation _representation;
		/// <summary>
		/// Gets the value representation (physical) of this open type.
		/// </summary>
		public OpenTypeRepresentation Representation
		{
			get { return _representation; }
		}
		#endregion

		#region Constructor
		protected OpenType(OpenTypeRepresentation representation, string typeName, string description)
		{
			_representation = representation;
			_name = typeName;
			_description = description;
		}
		#endregion

		#region Abstract
		public abstract bool IsValue(object value);		
		#endregion
	}

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
