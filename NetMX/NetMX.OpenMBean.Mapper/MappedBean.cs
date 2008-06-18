using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
   internal class MappedBean : NotificationEmitterSupport, IDynamicMBean
   {

      public MappedBean(MBeanInfo originalBeanInfo)
      {
         
      }

      #region IDynamicMBean Members
      public MBeanInfo GetMBeanInfo()
      {
         throw new NotImplementedException();
      }
      public object GetAttribute(string attributeName)
      {
         throw new NotImplementedException();
      }
      public void SetAttribute(string attributeName, object value)
      {
         throw new NotImplementedException();
      }
      public object Invoke(string operationName, object[] arguments)
      {
         throw new NotImplementedException();
      }
      #endregion
   }
}
