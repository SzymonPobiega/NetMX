using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NetMX.Server.InternalInfo
{
   internal sealed class MBeanInternalOperationInfo
   {
      #region PROPERTIES
      private MethodInfo _methodInfo;
      public MethodInfo MethodInfo
      {
         get { return _methodInfo; }
      }
      private MBeanOperationInfo _operationInfo;
      public MBeanOperationInfo OperationInfo
      {
         get { return _operationInfo; }
      }
      #endregion

      #region CONSTRUCTOR
      public MBeanInternalOperationInfo(MethodInfo method, IMBeanInfoFactory factory)
      {
         _methodInfo = method;
         _operationInfo = factory.CreateMBeanOperationInfo(method);
      }
      #endregion
   }
}