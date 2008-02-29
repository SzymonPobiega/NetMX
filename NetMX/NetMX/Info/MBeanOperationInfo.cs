#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
#endregion

namespace NetMX
{	
	[Flags]
	public enum OperationImpact
	{
		Unknown = 0,
		Info = 1,
		Action = 2
	}

	[Serializable]
	public class MBeanOperationInfo : MBeanFeatureInfo
	{		
		#region MEMBERS
		#endregion

		#region PROPERTIES
		private string _returnType;

		public string ReturnType
		{
			get { return _returnType; }
		}
		private MBeanParameterInfo[] _signature;

		public MBeanParameterInfo[] Signature
		{
			get { return _signature; }
		}
		private OperationImpact _impact;

		public OperationImpact Impact
		{
			get { return _impact; }
		}
		#endregion

		#region CONSTRUCTOR
		public MBeanOperationInfo(string name, string description, string returnType, MBeanParameterInfo[] signature, OperationImpact impact)
			: base(name, description)
		{
			_returnType = returnType;
			_signature = signature;
			_impact = impact;
		}
		public MBeanOperationInfo(MethodInfo info, OperationImpact impact) 
            : base (info.Name, InfoUtils.GetDescrition(info, "MBean operation"))
		{
			_returnType = info.ReturnType != null ? info.ReturnType.AssemblyQualifiedName : null;
			_impact = impact;
			ParameterInfo[] paramInfos = info.GetParameters();
			_signature = new MBeanParameterInfo[paramInfos.Length];
			for (int i = 0; i < paramInfos.Length; i++)
			{
                _signature[i] = new MBeanParameterInfo(paramInfos[i]);
			}
		}
		#endregion
	}
}
