using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Resources
{
    public class MBeanServerResource
    {
        public List<MBeanInfo> Beans { get; set; }
        public string InstanceName { get; set; }
        public string Version { get; set; }
    }
}