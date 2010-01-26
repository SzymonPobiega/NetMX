using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetMX.OpenMBean.Tests
{   
   [TestFixture]
   public class CompositeDataSupportTests
   {
      private static readonly CompositeType _sampleType = new CompositeType("Name", "Descr",
            new string[] { "Name1", "Name2" },
            new string[] { "Descr1", "Descr2" },
            new OpenType[] { SimpleType.Integer, SimpleType.Double });
      [Test]
      [ExpectedException(typeof(OpenDataException), "Names and value collections must have equal size.")]
      public void TestConstructorFailureNotEqualSizes()
      {     
         CompositeDataSupport data = new CompositeDataSupport(_sampleType, new string[] {"Name1", "Name2"}, new object[] {1});
      }
      [Test]
      [ExpectedException(typeof(OpenDataException), "Composite type doesn't have item with name Name3")]
      public void TestConstructorFailureNoSuchItem()
      {
         CompositeDataSupport data = new CompositeDataSupport(_sampleType, new string[] { "Name1", "Name3" }, new object[] { 1, 1.2 });
      }
      [Test]
      [ExpectedException(typeof(OpenDataException), "Value is not valid for its item's open type.")]
      public void TestConstructorFailureItemDataNotValid()
      {
         CompositeDataSupport data = new CompositeDataSupport(_sampleType, new string[] { "Name1", "Name2" }, new object[] { 1, true });
      }
      [Test]
      [ExpectedException(typeof(OpenDataException), "Composite type has different item count than count of items provided.")]
      public void TestConstructorFailureInvalidItemCount()
      {
         CompositeDataSupport data = new CompositeDataSupport(_sampleType, new string[] { "Name1" }, new object[] { 1 });
      }
      [Test]
      public void TestContainsKey()
      {
         CompositeDataSupport data = CreareSampleData();
         Assert.IsTrue(data.ContainsKey("Name1"));
         Assert.IsTrue(data.ContainsKey("Name2"));
         Assert.IsFalse(data.ContainsKey("Name3"));
      }
      [Test]
      public void TestContainsValue()
      {
         CompositeDataSupport data = CreareSampleData();
         Assert.IsTrue(data.ContainsValue(1));
         Assert.IsTrue(data.ContainsValue(1.2));
         Assert.IsFalse(data.ContainsValue(true));
      }
      [Test]
      public void TestIndexerSuccess()
      {
         CompositeDataSupport data = CreareSampleData();
         Assert.AreEqual(1, data["Name1"]);
         Assert.AreEqual(1.2, data["Name2"]);
      }
      [Test]
      [ExpectedException(typeof(InvalidKeyException))]
      public void TestIndexerFailure()
      {
         CompositeDataSupport data = CreareSampleData();
         object o = data["Name3"];
      }
      [Test]
      public void TestGetAllSuccess()
      {
         CompositeDataSupport data = CreareSampleData();
         IList<object> values = data.GetAll(new string[] {"Name1", "Name2"});
         Assert.AreEqual(2, values.Count);
         Assert.IsTrue(values.Contains(1));
         Assert.IsTrue(values.Contains(1.2));
      }
      [Test]
      [ExpectedException(typeof(InvalidKeyException))]
      public void TestGetAllFailure()
      {
         CompositeDataSupport data = CreareSampleData();
         IList<object> values = data.GetAll(new string[] { "Name1", "Name2", "Name3" });
      }

      #region Utility
      private static CompositeDataSupport CreareSampleData()
      {
         return new CompositeDataSupport(_sampleType, new string[] { "Name1", "Name2" }, new object[] { 1, 1.2 });
      }
      #endregion
   }
}
