#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#endregion

namespace NetMX
{
	[Serializable]
	public class MBeanAttributeInfo : MBeanFeatureInfo
	{
		#region MEMBERS		
		#endregion

		#region PROPERTIES
		private string _type;

		public string Type
		{
			get { return _type; }
		}
		private bool _isReadable;

		public bool Readable
		{
			get { return  _isReadable; }
		}
		private bool _isWritable;

		public bool Writable
		{
			get { return  _isWritable; }
		}		
		#endregion

		#region CONSTRUCTOR
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
		public MBeanAttributeInfo(PropertyInfo info)
			: this(info.Name, InfoUtils.GetDescrition(info, info, "MBean attribute"), info.PropertyType.AssemblyQualifiedName, info.CanRead, info.CanWrite)
		{           
		}        
		#endregion
	}
}
