using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetMX.OpenMBean.Tests
{
   /// <summary>
   /// Summary description for UnitTest1
   /// </summary>
   [TestClass]
   public class SimpleTypeTests
   {      
      #region Additional test attributes
      //
      // You can use the following additional attributes as you write your tests:
      //
      // Use ClassInitialize to run code before running the first test in the class
      // [ClassInitialize()]
      // public static void MyClassInitialize(TestContext testContext) { }
      //
      // Use ClassCleanup to run code after all tests in a class have run
      // [ClassCleanup()]
      // public static void MyClassCleanup() { }
      //
      // Use TestInitialize to run code before running each test 
      // [TestInitialize()]
      // public void MyTestInitialize() { }
      //
      // Use TestCleanup to run code after each test has run
      // [TestCleanup()]
      // public void MyTestCleanup() { }
      //
      #endregion

      [TestMethod]
      public void TestIsValue()
      {
         object[][] values = new object[][]
         {
            new object[] { SimpleType.Boolean, true},
            new object[] { SimpleType.Byte, (byte)5},
            new object[] { SimpleType.Character, 'h'},
            new object[] { SimpleType.DateTime, DateTime.Now},
            new object[] { SimpleType.Decimal, 5.6M},
            new object[] { SimpleType.Double, 6.67},
            new object[] { SimpleType.Float, 5.4f},
            new object[] { SimpleType.Integer, 56},
            new object[] { SimpleType.Long, 57L},
            new object[] { SimpleType.ObjectName, new ObjectName(":type=A")},
            new object[] { SimpleType.Short, (short)6},
            new object[] { SimpleType.String, "string"},
            new object[] { SimpleType.TimeSpan, TimeSpan.Zero},            
         };
         for (int i = 0; i < values.Length; i++)
         {
            for (int j = 0; j < values.Length; j++)
            {
               SimpleType st = (SimpleType) values[i][0];
               object value = values[j][1];
               if (i != j)
               {
                  Assert.IsFalse(st.IsValue(value));
               }
               else
               {
                  Assert.IsTrue(st.IsValue(value));
               }
            }
         }
      }
   }
}
