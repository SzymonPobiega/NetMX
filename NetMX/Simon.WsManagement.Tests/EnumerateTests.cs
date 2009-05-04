using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simon.WsManagement.Tests
{
   [TestClass]
   public class EnumerateTests
   {
      [TestMethod]
      public void CanSerializeAndDeserializeEnumerate()
      {
         XmlSerializer xs = new XmlSerializer(typeof(Enumerate));
         
         Enumerate underTest = new Enumerate(false, null);

         StringBuilder serializedForm = new StringBuilder();
         using (XmlWriter xw = XmlWriter.Create(serializedForm, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
         {
            xs.Serialize(xw, underTest);
         }
         Console.WriteLine(serializedForm.ToString());

         Enumerate deserializedResponse = (Enumerate)xs.Deserialize(new StringReader(serializedForm.ToString()));
         Assert.Inconclusive();
      }
   }
}
