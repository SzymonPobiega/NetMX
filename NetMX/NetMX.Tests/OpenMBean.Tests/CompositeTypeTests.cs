using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetMX.OpenMBean.Tests
{   
   [TestFixture]
   public class CompositeTypeTests
   {      
      [Test]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TestConstructorFailureNullItemNames()
      {         
         CompositeType type = new CompositeType("TypeName", "TypeDescription", 
            new string[] {"ItemName1", null},
            new string[] {"ItemDescr1", "ItemDescr2"},
            new OpenType[] { SimpleType.Integer, SimpleType.Double});
      }
      [Test]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TestConstructorFailureNullItemDescriptions()
      {
         CompositeType type = new CompositeType("TypeName", "TypeDescription",
            new string[] { "ItemName1", "ItemName2" },
            new string[] { "ItemDescr1", null },
            new OpenType[] { SimpleType.Integer, SimpleType.Double });         
      }
      [Test]
      [ExpectedException(typeof(ArgumentNullException))]
      public void TestConstructorFailureNullItemTypes()
      {
         CompositeType type = new CompositeType("TypeName", "TypeDescription",
            new string[] { "ItemName1", "ItemName2" },
            new string[] { "ItemDescr1", "ItemDescr2" },
            new OpenType[] { SimpleType.Integer, null });
      }
      [Test]
      [ExpectedException(typeof(OpenDataException), "CompositeType items cannot have duplicate keys.")]
      public void TestConstructorFailureDuplicateItems()
      {
         CompositeType type = new CompositeType("TypeName", "TypeDescription",
            new string[] { "ItemName", "ItemName" },
            new string[] { "ItemDescr1", "ItemDescr2" },
            new OpenType[] { SimpleType.Integer, SimpleType.Double });
      }      
      [Test]
      public void TestGetOpenType()
      {
         CompositeType type = CreateSampleType();
         Assert.AreEqual(SimpleType.Integer, type.GetOpenType("ItemName1"));
         Assert.AreEqual(SimpleType.Double, type.GetOpenType("ItemName2"));
         Assert.AreEqual(null, type.GetOpenType("ItemName3"));
      }
      [Test]
      public void TestGetDescription()
      {
         CompositeType type = CreateSampleType();
         Assert.AreEqual("ItemDescr1", type.GetDescription("ItemName1"));
         Assert.AreEqual("ItemDescr2", type.GetDescription("ItemName2"));
         Assert.AreEqual(null, type.GetDescription("ItemName3"));
      }
      [Test]
      public void TestContainsKey()
      {
         CompositeType type = CreateSampleType();
         Assert.IsTrue(type.ContainsKey("ItemName1"));
         Assert.IsTrue(type.ContainsKey("ItemName2"));
         Assert.IsFalse(type.ContainsKey("ItemName3"));
      }
      [Test]
      public void TestEquals()
      {         
         for (int i = 0; i < _testPairs.Length; i++)
         {
            CompositeType left = (CompositeType)_testPairs[i][0];
            CompositeType right = (CompositeType)_testPairs[i][1];
            bool result = (bool) _testPairs[i][2];
            Assert.AreEqual(result, left.Equals(right));
            Assert.AreEqual(result, right.Equals(left));
         }
      }
      [Test]
      public void TestEqualityOperator()
      {
         for (int i = 0; i < _testPairs.Length; i++)
         {
            CompositeType left = (CompositeType)_testPairs[i][0];
            CompositeType right = (CompositeType)_testPairs[i][1];
            bool result = (bool)_testPairs[i][2];
            Assert.AreEqual(result, left == right);
            Assert.AreEqual(result, right == left);            
         }
      }

      #region Utility
      private readonly object[][] _testPairs = new object[][]
         {
            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "OtherTypeDescription", new string[] { "Item2", "Item1" },new string[] { "Descr2", "Descr1" }, new OpenType[] { SimpleType.Double, SimpleType.Integer }), 
               true},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("OtherTypeName", "OtherTypeDescription", new string[] { "Item2", "Item1" },new string[] { "Descr2", "Descr1" }, new OpenType[] { SimpleType.Double, SimpleType.Integer }), 
               false},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2", "Item3" },new string[] { "Descr1", "Descr2", "Descr3" }, new OpenType[] { SimpleType.Integer, SimpleType.Double, SimpleType.Decimal }), 
               false},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "TypeDescription", new string[] { "Item1" },new string[] { "Descr1" }, new OpenType[] { SimpleType.Integer }), 
               false},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item3" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }), 
               false},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr3" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }), 
               true},

            new object[] { new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Double }),
               new CompositeType("TypeName", "TypeDescription", new string[] { "Item1", "Item2" },new string[] { "Descr1", "Descr2" }, new OpenType[] { SimpleType.Integer, SimpleType.Float }), 
               false}

         };
      private static CompositeType CreateSampleType()
      {
         return new CompositeType("TypeName", "TypeDescription",
            new string[] { "ItemName1", "ItemName2" },
            new string[] { "ItemDescr1", "ItemDescr2" },
            new OpenType[] { SimpleType.Integer, SimpleType.Double });
      }
      #endregion
   }
}
