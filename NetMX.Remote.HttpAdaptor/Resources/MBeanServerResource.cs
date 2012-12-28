using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Resources
{
    public class MBeanServerResource
    {
        public MBeanDomain RootDomain { get; set; }
        public string InstanceName { get; set; }
        public string Version { get; set; }
        public string StaticViewHref { get; set; }
        public string DynamicViewHref { get; set; }
    }
}