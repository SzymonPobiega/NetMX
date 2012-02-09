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
   /// Describes a constructor of an Open MBean.   
   /// </summary>
   [Serializable]
   public class OpenMBeanConstructorInfoSupport : IOpenMBeanConstructorInfo
   {
      private readonly MBeanConstructorInfo _wrappedInfo;
      private readonly IList<IOpenMBeanParameterInfo> _wrappedParameters;

      public OpenMBeanConstructorInfoSupport(MBeanConstructorInfo wrappedInfo)
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

      public IList<IOpenMBeanParameterInfo> Signature
      {
         get { return _wrappedParameters; }
      }
   }
}