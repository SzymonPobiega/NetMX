using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Representations
{
    public class MBeanServerRepresentation
    {
        public List<MBeanInfo> Beans { get; set; }
    }
}