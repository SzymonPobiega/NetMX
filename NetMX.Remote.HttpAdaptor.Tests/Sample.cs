namespace NetMX.Remote.HttpAdaptor.Tests
{
    public class Sample : SampleMBean
    {
        public string StringValue { get; set; }

        public Sample()
        {
            StringValue = "Just created";
        }
    }
}