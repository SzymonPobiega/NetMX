using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalAttributeInfo
	{
		#region PROPERTIES
		private PropertyInfo _property;
		public PropertyInfo Property
		{
			get { return _property; }
		}
		private MBeanAttributeInfo _attributeInfo;
		public MBeanAttributeInfo AttributeInfo
		{
			get { return _attributeInfo; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanInternalAttributeInfo(MBeanAttributeInfo attributeInfo, PropertyInfo property)
		{
			_attributeInfo = attributeInfo;
			_property = property;
		}
		#endregion
	}
}
