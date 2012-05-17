namespace NetMX.Remote.HttpAdaptor.Resources
{
    public class MBeanAttributeResource
    {
        public string Name { get; set; }
        public string MBeanHRef { get; set; }
        /// <summary>
        /// Can be either string (simple or enum type) or string[] (array type) or dictionary[] (tabular type)
        /// </summary>
        public object Value { get; set; }
    }
}