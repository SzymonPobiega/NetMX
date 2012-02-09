#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
	/// <summary>
	/// Represents an MBean attribute by associating its name with its value. The MBean server and other 
	/// objects use this class to get and set attributes values.
	/// </summary>
	[Serializable]
	public sealed class AttributeValue
	{
		#region PROPERTIES
		private string _name;
		/// <summary>
		/// Gets a String containing the name of the attribute.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}
		private object _value;
		/// <summary>
		/// Gets an Object that is the value of this attribute.
		/// </summary>
		public object Value
		{
			get { return _value; }
		}
		#endregion

		#region CONSTRUCTOR
		/// <summary>
		/// Constructs an Attribute object which associates the given attribute name with the given value.
		/// </summary>
		/// <param name="name">A String containing the name of the attribute to be created. Cannot be null.</param>
		/// <param name="value">The Object which is assigned to the attribute. This object must be of the same type as the attribute.</param>
		public AttributeValue(string name, object value)
		{
			_name = name;
			_value = value;
		}
		#endregion

		#region OVERRIDDEN
		/// <summary>
		/// Compares the current Attribute Object with another Attribute Object.
		/// </summary>
		/// <param name="obj">The Attribute that the current Attribute is to be compared with.</param>
		/// <returns>True if the two Attribute objects are equal, otherwise false.</returns>
		public override bool Equals(object obj)
		{
			AttributeValue other = obj as AttributeValue;
			return other != null && this.Name == other.Name &&
				((this.Value == null && other.Value == null) || (this.Value != null && other.Value != null &&
				this.Value.Equals(other.Value)));
		}		
		public override int GetHashCode()
		{
			int hashCode = _name.GetHashCode();
			if (_value != null)
			{
				hashCode ^= _value.GetHashCode();
			}
			return hashCode;
		}		
		#endregion
	}
}
