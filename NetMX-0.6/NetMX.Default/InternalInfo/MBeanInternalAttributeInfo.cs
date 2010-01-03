using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalAttributeInfo
	{
		#region PROPERTIES
		private readonly PropertyInfo _property;
		public PropertyInfo Property
		{
			get { return _property; }
		}
		private readonly MBeanAttributeInfo _attributeInfo;
		public MBeanAttributeInfo AttributeInfo
		{
			get { return _attributeInfo; }
		}
		#endregion

		#region CONSTRUCTOR
      public MBeanInternalAttributeInfo(PropertyInfo propInfo, IMBeanInfoFactory factory)
		{
         _property = propInfo;
         _attributeInfo = factory.CreateMBeanAttributeInfo(propInfo);
		}
		#endregion
	}
}
