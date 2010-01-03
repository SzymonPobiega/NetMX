using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simon.WsManagement.Tests
{
   [TestClass]
   public class SelectorTests
   {
      [TestMethod]
      public void SimpleSerizalizationTest()
      {
         Selector s = new Selector("NameA", "SimpleValue");
         MemoryStream mem = new MemoryStream();
         XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(mem, Encoding.Unicode);
         s.WriteTo(writer);
         writer.Flush();
         mem.Seek(0, SeekOrigin.Begin);
         StreamReader sr = new StreamReader(mem, Encoding.Unicode);
         string serializedText = sr.ReadToEnd();
         Assert.AreEqual(
@"<Selector Name=""NameA"" xmlns=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd"">SimpleValue</Selector>",
         serializedText);
      }
      [TestMethod]
      public void AddressReferenceSerizalizationTest()
      {
         AddressHeader ah1 = AddressHeader.CreateAddressHeader("RefParam1", "a", 5);
         AddressHeader ah2 = AddressHeader.CreateAddressHeader("RefParam2", "A", true);
         EndpointAddress addr = new EndpointAddress(new Uri("http://netmx.eu"), new AddressHeader[] { ah1, ah2 });
         Selector s = new Selector("NameA", addr);
         MemoryStream mem = new MemoryStream();
         XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(mem, Encoding.Unicode);
         s.WriteTo(writer);
         writer.Flush();
         mem.Seek(0, SeekOrigin.Begin);
         StreamReader sr = new StreamReader(mem, Encoding.Unicode);
         string serializedText = sr.ReadToEnd();
         Assert.AreEqual(
@"<Selector Name=""NameA"" xmlns=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd"">" +
 @"<EndpointReference xmlns=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">" +
     @"<Address>http://netmx.eu/</Address>" +
     @"<ReferenceParameters>" +
         @"<RefParam1 xmlns=""a"">5</RefParam1>" +
         @"<RefParam2 xmlns=""A"">true</RefParam2>" +
     @"</ReferenceParameters>" +
 @"</EndpointReference>" +
@"</Selector>",
         serializedText);
      }
      [TestMethod]
      public void NestedAddressReferenceSerizalizationTest()
      {
         ResourceUriHeader ah1 = new ResourceUriHeader("http://resource.uri");
         AddressHeader ah2 = new SelectorSetHeader(new Selector("NestedSelector", "NestedValue"));

         EndpointAddress addr = new EndpointAddress(new Uri("http://netmx.eu"), new AddressHeader[] { ah1, ah2 });
         Selector s = new Selector("NameA", addr);
         MemoryStream mem = new MemoryStream();
         XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(mem, Encoding.Unicode);
         s.WriteTo(writer);
         writer.Flush();
         mem.Seek(0, SeekOrigin.Begin);
         StreamReader sr = new StreamReader(mem, Encoding.Unicode);
         string serializedText = sr.ReadToEnd();
         Assert.AreEqual(
@"<Selector Name=""NameA"" xmlns=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd"">" +
 @"<EndpointReference xmlns=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">" +
     @"<Address>http://netmx.eu/</Address>" +
     @"<ReferenceParameters>" +
         @"<ResourceURI xmlns=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd"">http://resource.uri</ResourceURI>" +
         @"<SelectorSet xmlns=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd"">" +
             @"<Selector Name=""NestedSelector"">NestedValue</Selector>" +
         @"</SelectorSet>" +
     @"</ReferenceParameters>" +
 @"</EndpointReference>" +
@"</Selector>",
         serializedText);
      }
   }
}