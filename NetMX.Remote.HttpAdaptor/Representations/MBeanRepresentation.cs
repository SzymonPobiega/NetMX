using System.Collections.Generic;

namespace NetMX.Remote.HttpAdaptor.Representations
{
    public class MBeanRepresentation
    {
        public string ClassName { get; set; }
        public string Description { get; set; }
        public List<MBeanAttributeInfo> Attributes { get; set; }
    }
}