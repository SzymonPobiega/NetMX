namespace NetMX.Remote.HttpAdaptor.Resources
{
    public abstract class MBeanAttributeResource
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string MBeanHRef { get; set; }
    }

    public class MBeanSimpleValueAttributeResource : MBeanAttributeResource
    {
        public string SimpleValue { get; set; }
    }

    public class MBeanComplexValueAttributeResource : MBeanAttributeResource
    {
        public object ComplexValue { get; set; }
    }
}