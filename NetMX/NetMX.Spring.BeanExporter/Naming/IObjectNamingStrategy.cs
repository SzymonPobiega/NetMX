using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter.Naming
{
   public interface IObjectNamingStrategy
   {
      ObjectName GetObjectName(Object managedBean, String beanKey);
   }
}
