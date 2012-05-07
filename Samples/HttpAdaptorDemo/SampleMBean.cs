using NetMX.OpenMBean;

namespace HttpAdaptorDemo
{
    [OpenMBean]
    public interface SampleMBean
    {
        [OpenMBeanAttributeAttribute]
        string StringValue { get; set; }        
    }
}