using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter.Naming
{
   public class SimpleNamingStrategy : IObjectNamingStrategy
   {
      #region IObjectNamingStrategy Members
      public ObjectName GetObjectName(object managedBean, string beanKey)
      {
         Dictionary<string, string> properties = new Dictionary<string, string>();
         properties["name"] = beanKey;
         return new ObjectName("Domain", properties);
      }
      #endregion
   }
}
