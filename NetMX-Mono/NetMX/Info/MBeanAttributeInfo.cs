#region USING
using System;
using System.Reflection;
#endregion

namespace NetMX
{
	[Serializable]
	public class MBeanAttributeInfo : MBeanFeatureInfo
	{
		#region PROPERTIES
		private readonly string _type;
      /// <summary>
      /// Gets the class name of the attribute.
      /// </summary>
		public string Type
		{
			get { return _type; }
		}
		private readonly bool _isReadable;
      /// <summary>
      /// Tests whether the value of the attribute can be read.
      /// </summary>
		public bool Readable
		{
			get { return  _isReadable; }
		}
		private readonly bool _isWritable;
      /// <summary>
      /// Tests whether new values can be written to the attribute.
      /// </summary>
		public bool Writable
		{
			get { return  _isWritable; }
		}		
		#endregion

		#region CONSTRUCTOR
      /// <summary>
      /// Constructs an MBeanAttributeInfo object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">The type or class name of the attribute.</param>
      /// <param name="type">A human readable description of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
		public MBeanAttributeInfo(string name, string description, string type, bool isReadable, bool isWritable) : base(name, description)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (description == null)
			{
				throw new ArgumentNullException("description");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}			
			_type = type;
			_isReadable = isReadable;
			_isWritable = isWritable;
		}
      /// <summary>
      /// Constructs an MBeanAttributeInfo object.
      /// </summary>
      /// <param name="info">Property information object.</param>
		public MBeanAttributeInfo(PropertyInfo info)
			: this(info.Name, InfoUtils.GetDescrition(info, info, "MBean attribute"), info.PropertyType.AssemblyQualifiedName, info.CanRead, info.CanWrite)
		{           
		}        
		#endregion
	}
}
