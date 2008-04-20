#region Using
using System;
using System.Collections.Generic;
using System.Reflection;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes an operation of an Open MBean.   
   /// </summary>
   [Serializable]
   public class OpenMBeanOperationInfoSupport : MBeanOperationInfo, IOpenMBeanOperationInfo
   {
      #region Members
      private readonly OpenType _returnOpenType;
      #endregion

      #region Constructors
      /// <summary>
      /// Creates new OpenMBeanOperationInfoSupport object.
      /// </summary>
      /// <param name="name">The name of the method.</param>
      /// <param name="description">A human readable description of the operation.</param>
      /// <param name="returnOpenType">The open type of the method's return value.</param>
      /// <param name="signature">MBeanParameterInfo objects describing the parameters(arguments) of the method. It should be an empty list if operation has no parameters.</param>
      /// <param name="impact">The impact of the method.</param>
      public OpenMBeanOperationInfoSupport(string name, string description, OpenType returnOpenType, IEnumerable<IOpenMBeanParameterInfo> signature, OperationImpact impact)
			: base(name, description, returnOpenType.Representation.AssemblyQualifiedName, 
         OpenInfoUtils.Transform<MBeanParameterInfo, IOpenMBeanParameterInfo>(signature), impact, true)
		{
         _returnOpenType = returnOpenType;			
		}
      /// <summary>
      /// Creates new OpenMBeanOperationInfoSupport object.
      /// </summary>
      /// <param name="info">Method information object</param>
      public OpenMBeanOperationInfoSupport(MethodInfo info)
         : base(info, true)
      {
         object[] attrTmp = info.GetCustomAttributes(typeof (OpenMBeanOperationAttribute), false);
         if (attrTmp.Length == 0)
         {
            throw new OpenDataException("Open MBean operation have to have its impact specified.");
         }
         OpenMBeanOperationAttribute attr = (OpenMBeanOperationAttribute)attrTmp[0];
         if (attr.Impact == OperationImpact.Unknown)
         {
            throw new OpenDataException("Open MBean operation have to have its impact specified.");
         }
         _impact = attr.Impact;
         ParameterInfo[] paramInfos = info.GetParameters();
         List<MBeanParameterInfo> tmp = new List<MBeanParameterInfo>();
         for (int i = 0; i < paramInfos.Length; i++)
         {
            tmp.Add(new OpenMBeanParameterInfoSupport(paramInfos[i]));
         }
         _signature = tmp.AsReadOnly();
         _returnOpenType = info.ReturnType != null ? OpenType.CreateFromType(info.ReturnType) : SimpleType.Void;
      }
      #endregion

      #region IOpenMBeanOperationInfo Members      
      public OpenType ReturnOpenType
      {
         get { return _returnOpenType; }
      }
      #endregion
   }
}