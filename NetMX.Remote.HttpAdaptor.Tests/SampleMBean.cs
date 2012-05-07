using NetMX.OpenMBean;

namespace NetMX.Remote.HttpAdaptor.Tests
{
    [OpenMBean]
    public interface SampleMBean
    {
        [OpenMBeanAttributeAttribute]
        string StringValue { get; set; }        
    }
}