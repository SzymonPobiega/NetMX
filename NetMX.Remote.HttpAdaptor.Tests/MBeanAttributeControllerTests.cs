using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable PossibleNullReferenceException
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
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b/StringValue")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);
            Assert.AreEqual("Just created", resultObject["value"].Value<string>());
        }

        [Test]
        public void It_can_get_tabular_attribute_value_as_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Tabular")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);

            var array = (JArray) resultObject["value"];
            var firstRow = array[0];
            var secondRow = array[1];

            Assert.AreEqual(firstRow["ID"].Value<string>(), "1");
            Assert.AreEqual(firstRow["Name"].Value<string>(), "Simon");

            Assert.AreEqual(secondRow["ID"].Value<string>(), "2");
            Assert.AreEqual(secondRow["Name"].Value<string>(), "John");
        }

        [Test]
        public void It_can_get_composite_attribute_value_as_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Composite")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);

            var composite = (JObject)resultObject["value"];

            Assert.AreEqual("3", composite["ID"].Value<string>());
            Assert.AreEqual("Jane", composite["Name"].Value<string>());
        }

        [Test]
        public void It_can_get_array_attribute_value_as_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Array")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = JObject.Parse(resultString);

            var array = (JArray)resultObject["value"];
            var values = array.Select(x => x.Value<string>()).ToList();

            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("2.5", values[1]);
            Assert.AreEqual("4", values[2]);
            Assert.AreEqual("5", values[3]);
        }

        [Test]
        public void It_can_set_attribute_value_with_json()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/sample:a=b/StringValue", new StringContent(@"{ ""value"" : ""Ala ma kota"" }", Encoding.UTF8, "application/vnd.netmx.attr+json"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            var resultObject = JObject.Parse(resultString);
            Assert.AreEqual("Ala ma kota", resultObject["value"].Value<string>());
        }

        [Test]
        public void It_can_get_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/sample:a=b/StringValue")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var root = XElement.Parse(resultString);
            Assert.AreEqual("Just created", root.Element("Value").Value);
        }       

        [Test]
        public void It_can_get_tabular_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Tabular")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = XElement.Parse(resultString);

            var valueElement = resultObject.Element("Value");
            var tableElement = valueElement.Element("Tabular");
            var rows = tableElement.Elements("Row").ToList();
            var firstRow = rows[0];
            var secondRow = rows[1];

            Assert.AreEqual(firstRow.Element("ID").Value, "1");
            Assert.AreEqual(firstRow.Element("Name").Value, "Simon");

            Assert.AreEqual(secondRow.Element("ID").Value, "2");
            Assert.AreEqual(secondRow.Element("Name").Value, "John");
        }

        [Test]
        public void It_can_get_composite_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Composite")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = XElement.Parse(resultString);
            var valueElement = resultObject.Element("Value");
            var compositeElement = valueElement.Element("Composite");
            
            Assert.AreEqual(compositeElement.Element("ID").Value, "3");
            Assert.AreEqual(compositeElement.Element("Name").Value, "Jane");
        }

        [Test]
        public void It_can_get_array_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+json"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Array")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = XElement.Parse(resultString);
            var valueElement = resultObject.Element("Value");
            var arrayElement = valueElement.Element("Array");
            var values = arrayElement.Elements("Item").Select(x => x.Value).ToList();

            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("2.5", values[1]);
            Assert.AreEqual("4", values[2]);
            Assert.AreEqual("5", values[3]);
        }       

        [Test]
        public void It_can_set_attribute_value_with_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/sample:a=b/StringValue", new StringContent(@"<MBeanAttribute><Value>Ala ma kota</Value></MBeanAttribute>", Encoding.UTF8, "application/vnd.netmx.attr+xml"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            var root = XElement.Parse(resultString);
            Assert.AreEqual("Ala ma kota", root.Element("Value").Value);
        }

        [Test]
        public void It_can_get_and_set_composite_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.GetStringAsync("http://localhost:12345/adaptor/dynamic:a=b/Composite")
                .ContinueWith(x => resultString = x.Result)
                .Wait();

            var resultObject = XElement.Parse(resultString);
            var valueElement = resultObject.Element("Value");
            var compositeElement = valueElement.Element("Composite");

            compositeElement.Element("Name").Value = "Joe";

            var newXml = compositeElement.ToString();

            resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/dynamic:a=b/Composite", new StringContent(string.Format(@"<MBeanAttribute><Value>{0}</Value></MBeanAttribute>",newXml), Encoding.UTF8, "application/vnd.netmx.attr+xml"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            resultObject = XElement.Parse(resultString);
            valueElement = resultObject.Element("Value");
            compositeElement = valueElement.Element("Composite");

            Assert.AreEqual(compositeElement.Element("ID").Value, "3");
            Assert.AreEqual(compositeElement.Element("Name").Value, "Joe");
        }

        [Test]
        public void It_can_set_array_attribute_value_as_xml()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.netmx.attr+xml"));

            string resultString = "";
            client.PutAsync("http://localhost:12345/adaptor/dynamic:a=b/Array", new StringContent(@"<MBeanAttribute><Value><Array><Element>1</Element><Element>2.5</Element></Array></Value></MBeanAttribute>", Encoding.UTF8, "application/vnd.netmx.attr+xml"))
                .ContinueWith(x => x.Result.Content.ReadAsStringAsync().ContinueWith(y => resultString = y.Result).Wait())
                .Wait();

            var resultObject = XElement.Parse(resultString);
            var valueElement = resultObject.Element("Value");
            var arrayElement = valueElement.Element("Array");
            var values = arrayElement.Elements("Element").Select(x => x.Value).ToList();

            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("2.5", values[1]);
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

            Console.WriteLine(resultString);
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore PossibleNullReferenceException
