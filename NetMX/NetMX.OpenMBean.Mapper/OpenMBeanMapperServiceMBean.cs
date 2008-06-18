using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX.OpenMBean.Mapper
{
   public interface OpenMBeanMapperServiceMBean
   {
      ObjectName[] BeansToMapPatterns { get; set; }
      void RefreshMappings();
      void FlushMappedTypeCache();
   }
}
