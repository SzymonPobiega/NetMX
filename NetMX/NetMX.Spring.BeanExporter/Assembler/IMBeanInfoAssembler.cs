using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter.Assembler
{
   public interface IMBeanInfoAssembler
   {
      MBeanInfo GetMBeanInfo(object beanInstance, string beanKey);
   }
}
