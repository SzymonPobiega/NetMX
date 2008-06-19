using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
	/// <summary>
	/// Maps CLR types to <see cref="OpenType"/>s using a flavour of Chain of Responsibility pattern. 
	/// Individual type mapper objects are registered using integer priorities. When type mapping is invoked,
	/// mappers are queried (in order of rising priorities) if they can handle the type. First one which returns
	/// true is used. The types nested in the provided one are handled using the same chain.	
	/// </summary>
	/// <remarks>
	/// Caches generated <see cref="OpenTypes"/>s and maintains a dictionary mapping from CLR to Open types.
	/// </remarks>
	internal sealed class OpenTypeCache
	{
		#region Fields
		private readonly SortedList<int, ITypeMapper> _mappers = new SortedList<int, ITypeMapper>();
		private readonly Dictionary<Type, OpenType> _typeCache = new Dictionary<Type, OpenType>();
		#endregion

		#region Interface
		/// <summary>
		/// Adds a type mapper to the chain with given priority. If mapper privided as an external MBean, the
		/// <paramref name="objectName"/> arguments is the name of this MBean; otherwise it is null.
		/// </summary>
		/// <param name="mapper">The mapper instance. In case of external mapper this is a reference to proxy of the MBean.</param>
		/// <param name="objectName">In case of external mapper, this is its <see cref="ObjectName"/>.</param>
		/// <param name="priority">The priority of a mapper. Must be unique.</param>
		/// <exception cref="NonUniquePriorityException">Another mapper with privided priority is already registered.</exception>
		public void AddTypeMapper(ITypeMapper mapper, ObjectName objectName, int priority)
		{
		}
		/// <summary>
		/// Removes a type mapper from the chain.
		/// </summary>
		/// <param name="priority">Priority of mapper which should be removed.</param>
		public void RemoveTypeMapper(int priority)
		{
		}
		/// <summary>
		/// Gets the collection of type mapper information objects.
		/// </summary>
		/// <returns>A collection of type mapper information objects.</returns>
		public IEnumerable<TypeMapperInfo> GetTypeMappers()
		{
		}
		/// <summary>
		/// Flushes the mapped <see cref="OpenType"/> cache.
		/// </summary>
		public void FlushCache()
		{
			lock (_typeCache)
			{
				_typeCache.Clear();
			}
		}
		/// <summary>
		/// Maps the 
		/// </summary>
		/// <param name="clrType"></param>
		/// <returns></returns>
		public OpenType MapType(Type clrType)
		{
			lock (_typeCache)
			{
				OpenType mappedType;
				if (!_typeCache.TryGetValue(clrType, out mappedType))
				{					
					mappedType = DoTypeMapping(clrType);
					_typeCache[clrType] = mappedType;					
				}
				return mappedType;
			}
		}
		#endregion

		#region Utility
		private OpenType DoTypeMapping(Type clrType)
		{						
		}
		private OpenType MapTypeImpl(Type plainNetType)
		{
			OpenTypeKind mapsTo;
			foreach (ITypeMapper mapper in _mappers)
			{
				if (mapper.CanHandle(clrType, out mapsTo, CanHandleImpl))
				{
					return mapper.MapType(clrType, MapTypeImpl);
				}
			}
			return null;
		}
		private bool CanHandleImpl(Type plainNetType, out OpenTypeKind mapsTo)
		{
			OpenTypeKind mapsTo;
			foreach (ITypeMapper mapper in _mappers)
			{
				if (mapper.CanHandle(clrType, out mapsTo, CanHandleImpl))
				{
					return true;
				}
			}
			return false;
		}
		private object MapValueImpl(OpenType mappedType, object value)
		{			
		}
		#endregion
	}	
}
