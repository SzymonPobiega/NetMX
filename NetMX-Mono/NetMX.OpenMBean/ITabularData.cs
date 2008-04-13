using System.Collections.Generic;

namespace NetMX.OpenMBean
{
	/// <summary>
	/// The ITabularData interface specifies the behavior of a specific type of complex open data objects 
	/// which represent tabular data structures.
	/// </summary>
	public interface ITabularData
	{
		/// <summary>
		/// Calculates the index that would be used in this ITabularData instance to refer to the specified composite 
		/// data value parameter if it were added to this instance. This method checks for the type validity of the 
		/// specified value, but does not check if the calculated index is already used to refer to a value in this 
		/// ITabularData instance.
		/// </summary>
		/// <param name="value">The composite data value whose index in this ITabularData instance is to be calculated; 
		/// must be of the same composite type as this instance's row type; must not be null.</param>
		/// <returns>The index that the specified value would have in this ITabularData instance.</returns>
		/// <exception cref="NetMX.OpenMBean.InvalidOpenTypeException">If <paramref name="value"/> does not conform to 
		/// this ITabularData instance's row type definition.</exception>
		IList<object> CalculateIndex(ICompositeData value);
		/// <summary>
		/// Removes all <see cref="NetMX.OpenMBean.ICompositeData"/> values (rows) from this ITabularData instance.
		/// </summary>
		void Clear();
		/// <summary>
		/// Returns true if and only if this ITabularData instance contains a <see cref="NetMX.OpenMBean.ICompositeData"/> value (ie a row) 
		/// whose index is the specified key. If key is null or does not conform to this ITabularData instance's 
		/// <see cref="NetMX.OpenMBean.TabularType"/> definition, this method simply returns false.
		/// </summary>
		/// <param name="key">The index value whose presence in this ITabularData instance is to be tested.</param>
		/// <returns>True if this ITabularData indexes a row value with the specified key.</returns>
		bool ContainsKey(IEnumerable<object> key);
		/// <summary>
		/// Returns true if and only if this ITabularData instance contains the specified 
		/// <see cref="NetMX.OpenMBean.ICompositeData"/> value. If value is null or does not conform to this 
		/// ITabularData instance's row type definition, this method simply returns false.
		/// </summary>
		/// <param name="value">The row value whose presence in this ITabularData instance is to be tested.</param>
		/// <returns>True if this ITabularData instance contains the specified row value.</returns>
		bool ContainsValue(ICompositeData value);
		/// <summary>
		/// Gets the <see cref="NetMX.OpenMBean.ICompositeData"/> value whose index is key, or null if there is no 
      /// value mapping to key, in this ITabularData instance or the key does not conform to this ITabularData
      /// instance's <see cref="NetMX.OpenMBean.TabularType"/> definition</exception>
		/// </summary>
		/// <param name="key">The key of the row to return.</param>
		/// <returns>The value corresponding to key.</returns>		
		ICompositeData this[IEnumerable<object> key] { get; }
		/// <summary>
		/// Gets the tabular type describing this ITabularData instance.
		/// </summary>
		TabularType TabularType { get; }
		/// <summary>
		/// Returns true if the number of <see cref="NetMX.OpenMBean.ICompositeData"/>  values (the number of rows) 
		/// contained in this ITabularData instance is zero.
		/// </summary>
		bool Empty { get; }
		/// <summary>
		/// Gets the keys (the index values) of the <see cref="NetMX.OpenMBean.ICompositeData"/> values (the rows) 
		/// contained in this ITabularData instance. 
		/// </summary>
		IEnumerable<IEnumerable<object>> Keys { get; }
		/// <summary>
		/// Adds value to this ITabularData instance. The composite type of value must be the same as this 
		/// instance's row type (the composite type returned by this.TabularType.RowType), and there must not 
		/// already be an existing value in this ITabularData instance whose index is the same as the one calculated 
		/// for the value to be added. The index for value is calculated according to this ITabularData instance's 
		/// <see cref="NetMX.OpenMBean.TabularType"/>  definition (see <see cref="NetMX.OpenMBean.TabularType.IndexNames"/>).
		/// </summary>
		/// <param name="value">The composite data value to be added as a new row to this ITabularData instance; 
		/// must be of the same composite type as this instance's row type; must not be null.</param>
		/// <exception cref="NetMX.OpenMBean.KeyAlreadyExistsException">If the index for value, calculated according 
		/// to this ITabularData instance's <see cref="NetMX.OpenMBean.TabularType"/> definition already maps to an 
		/// existing value in the underlying dictionary.</exception>
		/// <exception cref="NetMX.OpenMBean.InvalidOpenTypeException">If value does not conform to this ITabularData 
		/// instance's row type definition.</exception>
		void Put(ICompositeData value);
		/// <summary>
		/// Add all the elements in values to this ITabularData instance. If any element in values does not satisfy 
		/// the constraints defined in put, or if any two elements in values have the same index calculated according
		/// to this ITabularData instance's <see cref="NetMX.OpenMBean.TabularType"/> definition, then an exception 
		/// describing the failure is thrown and no element of values is added, thus leaving this ITabularData 
		/// instance unchanged.
		/// </summary>
		/// <param name="values">The array of composite data values to be added as new rows to this ITabularData 
		/// instance; if values is null or empty, this method returns without doing anything.</param>
		void PutAll(IEnumerable<ICompositeData> values);
		/// <summary>
		/// Removes the <see cref="NetMX.OpenMBean.ICompositeData"/> value whose index is key from this ITabularData instance, and returns the 
		/// removed value, or returns null if there is no value whose index is key.
		/// </summary>
		/// <param name="key">The index of the value to get in this ITabularData instance; must be valid with 
		/// this ITabularData instance's row type definition; must not be null.</param>
		/// <returns></returns>		
		ICompositeData Remove(IEnumerable<object> key);
		/// <summary>
		/// Gets the number of <see cref="NetMX.OpenMBean.ICompositeData"/> values (the number of rows) contained 
		/// in this ITabularData  instance.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// Gets a collection view of the <see cref="NetMX.OpenMBean.ICompositeData"/> values (the rows) contained 
		/// in this ITabularData instance. The returned collection can then be used to iterate over the values.
		/// </summary>
		IEnumerable<ICompositeData> Values { get; }
	}
}
