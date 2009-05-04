using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simon.WsManagement.Tests
{
   [TestClass]
   public class EnumerateResponseTests
   {
      [TestMethod]
      public void CanSerializeAndDeserializeEnumerateResponse()
      {
         XmlSerializer xs = new XmlSerializer(typeof(EnumerateResponse));
         List<EndpointAddress> list = new List<EndpointAddress>()
                                           {
                                              new EndpointAddress("http://vsoft.pl"),
                                              new EndpointAddress("http://netmx.eu")
                                           };
         EnumerateResponse underTest = new EnumerateResponse(list);
         StringBuilder serializedForm = new StringBuilder();
         using (XmlWriter xw = XmlWriter.Create(serializedForm, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
         {
            xs.Serialize(xw, underTest);
         }
         Console.WriteLine(serializedForm.ToString());

         EnumerateResponse deserializedResponse = (EnumerateResponse)xs.Deserialize(new StringReader(serializedForm.ToString()));
         Assert.Inconclusive();
      }
   }
}
