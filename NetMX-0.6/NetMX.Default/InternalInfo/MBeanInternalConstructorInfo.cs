using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalConstructorInfo
	{
		#region PROPERTIES
		private readonly ConstructorInfo _methodInfo;
      public ConstructorInfo MethodInfo
		{
         get { return _methodInfo; }
		}
		private readonly MBeanConstructorInfo _constructorInfo;
      public MBeanConstructorInfo ConstructorInfo
		{
			get { return _constructorInfo; }
		}
		#endregion

		#region CONSTRUCTOR
      public MBeanInternalConstructorInfo(ConstructorInfo method, IMBeanInfoFactory factory)
		{
			_methodInfo = method;
         _constructorInfo = factory.CreateMBeanConstructorInfo(method);
		}
		#endregion
	}
}
