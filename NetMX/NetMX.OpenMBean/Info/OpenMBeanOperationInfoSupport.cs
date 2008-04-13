#region Using
using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// 
   /// </summary>
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
         : base(info)
      {         
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