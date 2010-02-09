using System;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Represents metadata about MBean attribute.
   /// </summary>
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
      public MBeanAttributeInfo(string name, string description, string type, bool isReadable, bool isWritable)
         : this(name, description, type, isReadable, isWritable, new Descriptor())
      {         
      }
      /// <summary>
      /// Constructs an MBeanAttributeInfo object.
      /// </summary>
      /// <param name="name">The name of the attribute.</param>
      /// <param name="description">The type or class name of the attribute.</param>
      /// <param name="type">A human readable description of the attribute.</param>
      /// <param name="isReadable">True if the attribute has a getter method, false otherwise.</param>
      /// <param name="isWritable">True if the attribute has a setter method, false otherwise.</param>
      /// <param name="descriptor">Initial descriptor values.</param>
      public MBeanAttributeInfo(string name, string description, string type, bool isReadable, bool isWritable, Descriptor descriptor)
         : base(name, description, descriptor)
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
		#endregion
	}
}
