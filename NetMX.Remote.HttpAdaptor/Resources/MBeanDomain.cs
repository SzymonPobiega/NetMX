using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Resources
{
    public class MBeanDomain
    {
        public string Name { get; set; }
        public List<MBeanInfo> Beans { get; set; }
        public List<MBeanDomain> Subdomains { get; set; }

        public MBeanDomain()
        {
            Beans = new List<MBeanInfo>();
            Subdomains = new List<MBeanDomain>();
        }
    }
}