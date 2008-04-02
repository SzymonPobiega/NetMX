using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Default.InternalInfo
{
	internal sealed class MBeanInternalConstructorInfo
	{
		#region PROPERTIES
		private ConstructorInfo _methodInfo;
      public ConstructorInfo MethodInfo
		{
         get { return _methodInfo; }
		}
		private MBeanConstructorInfo _constructorInfo;
      public MBeanConstructorInfo ConstructorInfo
		{
			get { return _constructorInfo; }
		}
		#endregion

		#region CONSTRUCTOR
      public MBeanInternalConstructorInfo(MBeanConstructorInfo constructorInfo, ConstructorInfo method)
		{
			_methodInfo = method;
			_constructorInfo = constructorInfo;
		}
		#endregion
	}
}
