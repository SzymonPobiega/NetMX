using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Resources
{
    public class MBeanResource
    {
        public string ClassName { get; set; }
        public string Description { get; set; }
        public List<MBeanAttributeInfo> Attributes { get; set; }
        public string ServerHRef { get; set; }
        public List<MBeanRelationInfo> Relations { get; set; }
    }
}