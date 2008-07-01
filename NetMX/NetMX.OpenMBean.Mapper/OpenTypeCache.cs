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
	/// Caches generated <see cref="OpenType"/>s and maintains a dictionary mapping from CLR to Open types.
	/// </remarks>
	internal sealed class OpenTypeCache
	{
		#region Fields
		private readonly SortedList<int, ITypeMapper> _mappers = new SortedList<int, ITypeMapper>();
		private readonly Dictionary<Type, OpenType> _typeCache = new Dictionary<Type, OpenType>();
      private readonly List<TypeMapperInfo> _mapperInfos = new List<TypeMapperInfo>();
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
         TypeMapperInfo newMapperInfo = new TypeMapperInfo(priority, objectName == null ? mapper.GetType().AssemblyQualifiedName : null, objectName);         
         if (_mappers.ContainsKey(priority))
         {
            TypeMapperInfo exisingMapperInfo = _mapperInfos.Find(delegate(TypeMapperInfo info)
              {
                 return info.Priority == priority;
              });
            throw new NonUniquePriorityException(
               newMapperInfo.ObjectName != null ? newMapperInfo.ObjectName.ToString() : newMapperInfo.TypeName,
               exisingMapperInfo.ObjectName != null ? exisingMapperInfo.ObjectName.ToString() : exisingMapperInfo.TypeName,
               priority
               ); 
         }
         _mappers.Add(priority, mapper);
         _mapperInfos.Add(newMapperInfo);
		}
		/// <summary>
		/// Removes a type mapper from the chain.
		/// </summary>
		/// <param name="priority">Priority of mapper which should be removed.</param>
		public void RemoveTypeMapper(int priority)
		{
         if (!_mappers.ContainsKey(priority))
         {
            throw new MapperNotFoundException(priority);
         }
		   _mappers.Remove(priority);
		}
		/// <summary>
		/// Gets the collection of type mapper information objects.
		/// </summary>
		/// <returns>A collection of type mapper information objects.</returns>
		public IEnumerable<TypeMapperInfo> GetTypeMappers()
		{
		   return new List<TypeMapperInfo>(_mapperInfos).AsReadOnly();
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
					mappedType = MapTypeImpl(clrType);
					_typeCache[clrType] = mappedType;					
				}
				return mappedType;
			}
		}
      /// <summary>
      /// Maps value.
      /// </summary>
      /// <param name="clrType"></param>
      /// <param name="mappedType"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public object MapValue(Type clrType, OpenType mappedType, object value)
      {
         lock (_typeCache)
         {
            return MapValueImpl(clrType, mappedType, value);
         }
      }
		#endregion

		#region Utility		
		private OpenType MapTypeImpl(Type plainNetType)
		{
			OpenTypeKind mapsTo;
			foreach (ITypeMapper mapper in _mappers.Values)
			{
            if (mapper.CanHandle(plainNetType, out mapsTo, CanHandleImpl))
				{
               return mapper.MapType(plainNetType, MapTypeImpl);
				}
			}
			return null;
		}
		private bool CanHandleImpl(Type plainNetType, out OpenTypeKind mapsTo)
		{
		   mapsTo = OpenTypeKind.SimpleType;
			foreach (ITypeMapper mapper in _mappers.Values)
			{
            if (mapper.CanHandle(plainNetType, out mapsTo, CanHandleImpl))
				{
					return true;
				}
			}
			return false;
		}
		private object MapValueImpl(Type plainNetType, OpenType mappedType, object value)
		{
		   OpenTypeKind mapsTo;
		   foreach (ITypeMapper mapper in _mappers.Values)
		   {
		      if (mapper.CanHandle(plainNetType, out mapsTo, CanHandleImpl))
		      {
		         return mapper.MapValue(plainNetType, mappedType, value, MapValueImpl);
		      }
		   }
		   return null;
		}
	   #endregion
	}	
}
