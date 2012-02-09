#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes an operation of an Open MBean.   
   /// </summary>
   [Serializable]
   public class OpenMBeanOperationInfoSupport : IOpenMBeanOperationInfo
   {
      private readonly MBeanOperationInfo _wrappedInfo;
      private readonly IList<IOpenMBeanParameterInfo> _wrappedParameters;

      /// <summary>
      /// Creates new instance of <see cref="OpenMBeanOperationInfoSupport"/> wrapping provided <see cref="MBeanOperationInfo"/>.
      /// </summary>
      /// <param name="wrappedInfo"></param>
      public OpenMBeanOperationInfoSupport(MBeanOperationInfo wrappedInfo)
      {
         _wrappedInfo = wrappedInfo;
         _wrappedParameters = _wrappedInfo.Signature.Select<MBeanParameterInfo, IOpenMBeanParameterInfo>(x => new OpenMBeanParameterInfoSupport(x)).ToList().AsReadOnly();
      }

      public string Name
      {
         get { return _wrappedInfo.Name; }
      }

      public string Description
      {
         get { return _wrappedInfo.Description; }
      }

      public OperationImpact Impact
      {
         get { return _wrappedInfo.Impact; }
      }

      public OpenType ReturnOpenType
      {
         get { return _wrappedInfo.Descriptor.GetFieldValue(OpenTypeDescriptor.Field); }
      }

      public string ReturnType
      {
         get { return _wrappedInfo.ReturnType; }
      }

      public IList<IOpenMBeanParameterInfo> Signature
      {
         get { return _wrappedParameters; }
      }
   }
}