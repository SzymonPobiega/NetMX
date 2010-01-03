#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
#endregion

namespace NetMX
{
	[Serializable]
	public class MBeanParameterInfo : MBeanFeatureInfo
	{
		#region MEMBERS
		#endregion

		#region PROPERTIES
		private readonly string _type;

		public string Type
		{
			get { return _type; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanParameterInfo(string name, string description, string type)
			: base(name, description)
		{
			_type = type;
		}
		public MBeanParameterInfo(ParameterInfo paramInfo)
			: base(paramInfo.Name, InfoUtils.GetDescrition(paramInfo.Member, paramInfo, "MBean operation parameter", paramInfo.Name))
		{
			_type = paramInfo.ParameterType.AssemblyQualifiedName;
		}
		#endregion
	}
}