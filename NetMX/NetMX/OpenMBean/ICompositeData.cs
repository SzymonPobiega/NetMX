using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean
{
	/// <summary>
	/// The ICompositeData interface specifies the behavior of a specific type of complex open data objects 
	/// which represent composite data structures.
	/// </summary>
	public interface ICompositeData
	{
		/// <summary>
		/// Returns true if and only if this ICompositeData instance contains an item whose name is key. If key 
		/// is a null or empty String, this method simply returns false.
		/// </summary>
		/// <param name="key">The key to be tested.</param>
		/// <returns>Ttrue if this ICompositeData contains the key.</returns>
		bool ContainsKey(string key);
		/// <summary>
		/// Returns true if and only if this ICompositeData instance contains an item whose value is value.
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		/// <returns>True if this CompositeData contains the value.</returns>
		bool ContainsValue(object value);
		/// <summary>
		/// Gets the value of the item whose name is key.
		/// </summary>
		/// <param name="value">The name of the item.</param>
		/// <returns>The value associated with this key.</returns>
		/// <exception cref="NetMX.OpenMBean.InvalidKeyException">If an element in keys is not an existing item name for this CompositeData instance.</exception>		
		object this[string value] { get; }
		/// <summary>
		/// Gets the values of the items whose names are specified by keys, in the same order as keys.
		/// </summary>
		/// <param name="keys">The names of the items.</param>
		/// <returns>The values corresponding to the keys.</returns>
		/// <exception cref="NetMX.OpenMBean.InvalidKeyException">If an element in keys is not an existing item name for this CompositeData instance.</exception>		
		IList<object> GetAll(IEnumerable<string> keys);
		/// <summary>
		/// Gets the composite type  of this composite data instance.
		/// </summary>
		CompositeType CompositeType { get; }
		/// <summary>
		/// Returns an unmodifiable list view of the item values contained in this ICompositeData instance. 
		/// Values are returned in ascending lexicographic order of their correponding keys.		
		/// </summary>
		IEnumerable<object> Values { get; }
	}
}
