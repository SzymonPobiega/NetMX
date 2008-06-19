using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
	/// <summary>
	/// Provides information about a type mapper object.
	/// </summary>
	public sealed class TypeMapperInfo
	{
		private int _priority;
		private string _typeName;
		private ObjectName _objectName;

		/// <summary>
		/// Gets the priority of a mapper. Mappers are queried for handling types from lowest priorities to highest.
		/// </summary>
		public int Priority
		{
			get { return _priority; }
		}

		/// <summary>
		/// Gets the CLR type name of a mapper, if it was registered as an internal mapper (not an MBean).
		/// </summary>
		public string TypeName
		{
			get { return _typeName; }
		}

		/// <summary>
		/// Gets the <see cref="ObjectName"/> of a mapper, if it was registeref as an external mapper (MBean).
		/// </summary>
		public ObjectName ObjectName
		{
			get { return _objectName; }
		}
	}
}
