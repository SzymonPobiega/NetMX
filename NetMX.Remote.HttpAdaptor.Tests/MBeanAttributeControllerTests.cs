using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Remote.HttpAdaptor.Tests
{
    [TestFixture]
    public class MBeanAttributeControllerTests: TestBase
    {
        [Test]
        public void It_can_get_attribute_value_as_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr.simple+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b/StringValue")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);
            Assert.AreEqual("Just created", resultObject["value"].Value<string>());
        }

        [Test]
        public void It_can_get_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr.simple+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b/StringValue")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var root = XElement.Parse(resultString);
            Assert.AreEqual("Just created", root.Element("Value").Value);
        }

        [Test]
        public void It_can_get_attribute_value_as_html()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b/StringValue")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            //var root = XElement.Parse(resultString);
            //Assert.AreEqual("Just created", root.Element("Value").Value);
            Console.WriteLine(resultString);
            Thread.Sleep(1000000);
        }

        [Test]
        public void It_can_set_attribute_value_with_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr.simple+json"));

            string resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/sample:a=b/StringValue", new StringContent(@"{ ""type"" : ""String"", ""value"" : ""Ala ma kota"" }", Encoding.UTF8, "application/vnd.netmx.attr.simple+json"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            var resultObject = JObject.Parse(resultString);
            Assert.AreEqual("Ala ma kota", resultObject["value"].Value<string>());
        }

        [Test]
        public void It_can_set_attribute_value_with_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr.simple+xml"));

            string resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/sample:a=b/StringValue", new StringContent(@"<MBeanAttribute><Type>String</Type><Value>Ala ma kota</Value></MBeanAttribute>", Encoding.UTF8, "application/vnd.netmx.attr.simple+xml"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            var root = XElement.Parse(resultString);
            Assert.AreEqual("Ala ma kota", root.Element("Value").Value);
        }

    }
}
// ReSharper restore InconsistentNaming
