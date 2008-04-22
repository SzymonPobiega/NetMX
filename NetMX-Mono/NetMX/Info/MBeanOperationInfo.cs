#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
#endregion

namespace NetMX
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue"), Flags]
	public enum OperationImpact
	{
		Unknown = 0,
		Info = 1,
		Action = 2
	}

	[Serializable]
	public class MBeanOperationInfo : MBeanFeatureInfo
	{		
		#region PROPERTIES
		private readonly string _returnType;
      /// <summary>
      /// 
      /// </summary>
		public string ReturnType
		{
			get { return _returnType; }
		}
      private ReadOnlyCollection<MBeanParameterInfo> _signature;
      /// <summary>
      /// 
      /// </summary>
		public IList<MBeanParameterInfo> Signature
		{
			get { return _signature; }
         protected set { _signature = value as ReadOnlyCollection<MBeanParameterInfo>; } 
		}
      private OperationImpact _impact;
      /// <summary>
      /// 
      /// </summary>
		public OperationImpact Impact
		{
			get { return _impact; }
         protected set { _impact = value; } 
		}
		#endregion

		#region CONSTRUCTOR
      /// <summary>
      /// Creates new MBeanOperationInfo object.
      /// </summary>
      /// <param name="name">The name of the method.</param>
      /// <param name="description">A human readable description of the operation.</param>
      /// <param name="returnType">The type of the method's return value.</param>
      /// <param name="signature">MBeanParameterInfo objects describing the parameters(arguments) of the method. It should be an empty list if operation has no parameters.</param>
      /// <param name="impact">The impact of the method.</param>
		public MBeanOperationInfo(string name, string description, string returnType, IEnumerable<MBeanParameterInfo> signature, OperationImpact impact)
			: base(name, description)
		{
			_returnType = returnType;
		   _signature = new List<MBeanParameterInfo>(signature).AsReadOnly();
			_impact = impact;
		}
      /// <summary>
      /// Creates new MBeanOperationInfo object.
      /// </summary>
      /// <param name="name">The name of the method.</param>
      /// <param name="description">A human readable description of the operation.</param>
      /// <param name="returnType">The type of the method's return value.</param>
      /// <param name="signature">MBeanParameterInfo objects describing the parameters(arguments) of the method. It should be an empty list if operation has no parameters.</param>
      /// <param name="impact">The impact of the method.</param>
      /// <param name="dummy">A dummy parameter used to differenciate constructor signatures.</param>
      protected MBeanOperationInfo(string name, string description, string returnType, ReadOnlyCollection<MBeanParameterInfo> signature, OperationImpact impact, bool dummy)
         : base(name, description)
      {
         _returnType = returnType;
         _signature = signature;
         _impact = impact;
      }
      /// <summary>
      /// Creates new MBeanOperationInfo object using reflection.
      /// </summary>
      /// <param name="info">Method information object.</param>
      /// <param name="dummy">A dummy parameter used to differenciate constructor signatures.</param>
      protected MBeanOperationInfo(MethodInfo info, bool dummy)
			: base(info.Name, InfoUtils.GetDescrition(info, info, "MBean operation"))
      {
         _returnType = info.ReturnType != null ? info.ReturnType.AssemblyQualifiedName : null;         
      }
      /// <summary>
      /// Creates new MBeanOperationInfo object using reflection.
      /// </summary>
      /// <param name="info">Method information object.</param>
		public MBeanOperationInfo(MethodInfo info)
			: base(info.Name, InfoUtils.GetDescrition(info, info, "MBean operation"))
		{
			_returnType = info.ReturnType != null ? info.ReturnType.AssemblyQualifiedName : null;
			_impact = OperationImpact.Unknown;
			ParameterInfo[] paramInfos = info.GetParameters();
			List<MBeanParameterInfo> tmp = new List<MBeanParameterInfo>();			
			for (int i = 0; i < paramInfos.Length; i++)
			{
				tmp.Add(new MBeanParameterInfo(paramInfos[i]));
			}
			_signature = tmp.AsReadOnly();
		}
		#endregion
	}
}
