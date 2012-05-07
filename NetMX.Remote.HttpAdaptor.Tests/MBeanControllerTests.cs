using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Remote.HttpAdaptor.Tests
{
    [TestFixture]
    public class MBeanControllerTests : TestBase
    {
        [Test]
        public void It_can_get_MBean_info_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.bean+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var root = XElement.Parse(resultString);
            var className = root.Element("ClassName");
            Assert.AreEqual("NetMX.Remote.HttpAdaptor.Tests.SampleMBean, NetMX.Remote.HttpAdaptor.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                className.Value);
        }

        [Test]
        public void It_can_get_MBean_info_as_html()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            //var root = XElement.Parse(resultString);
            Console.WriteLine(resultString);
        }

        [Test]
        public void It_can_get_MBean_info_as_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.bean+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);
            Assert.AreEqual("NetMX.Remote.HttpAdaptor.Tests.SampleMBean, NetMX.Remote.HttpAdaptor.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 
                resultObject["ClassName"].Value<string>());
        }
    }
}
// ReSharper restore InconsistentNaming
