using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetMX.OpenMBean.Tests
{
   [TestClass]
   public class ArrayTypeTests
   {
      [TestMethod]
      public void TestIsValue()
      {
         object[][] values = new object[][]
            {
               new object[] {new int[4], true, "int[4]" }, 
               new object[] {new bool[4], false , "bool[4]"},
               new object[] {new int[2,2] {{1, 2},{3,4}}, false, "int[2,2] {{1, 2},{3,4}}"},
               new object[] {new int[][] {new int[] {1,2},new int[] {3,4}}, false, "int[] {1,2},new int[] {3,4}}"}, 
               new object[] {new object[] {1,2}, true, "object[] {1,2}"}, 
               new object[] {new object[] {1,true}, false, "object[] {1,true}"}, 
               new object[] {new object[] {1,null}, true, "object[] {1,null}"}, 
               new object[] {new object[,] {{1,2},{3,4}}, false, "object[,] {{1,2},{3,4}}"}, 
            };
         ArrayType type = new ArrayType(1, SimpleType.Integer);
         for (int i = 0; i < values.Length; i++)
         {
            object value = values[i][0];
            bool result = (bool)values[i][1];
            string descr = (string) values[i][2];
            Assert.AreEqual(result, type.IsValue(value), descr);
         }
      }
      [TestMethod]
      public void TestEquals()
      {
         Assert.IsTrue(new ArrayType(1, SimpleType.Integer).Equals(new ArrayType(1, SimpleType.Integer)));
         Assert.IsTrue(new ArrayType(2, SimpleType.Double).Equals(new ArrayType(2, SimpleType.Double)));
         Assert.IsTrue(new ArrayType(20, SimpleType.ObjectName).Equals(new ArrayType(20, SimpleType.ObjectName)));

         Assert.IsFalse(new ArrayType(1, SimpleType.Integer).Equals(new ArrayType(2, SimpleType.Integer)));
         Assert.IsFalse(new ArrayType(1, SimpleType.Integer).Equals(new ArrayType(1, SimpleType.Double)));
      }
   }
}
