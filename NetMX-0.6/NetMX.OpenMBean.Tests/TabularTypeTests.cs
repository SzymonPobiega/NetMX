using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetMX.OpenMBean.Tests
{   
   [TestClass]
   public class TabularTypeTests
   {
      private static readonly CompositeType _compType1 = new CompositeType("Type1", "Type1", new string[] {"Item1", "Item2"}, new string[] {"Descr1", "Descr2"}, new OpenType[] {SimpleType.Integer, SimpleType.Double});
      private static readonly CompositeType _compType2 = new CompositeType("Type2", "Type2", new string[] {"Item1", "Item2"}, new string[] {"Descr1", "Descr2"}, new OpenType[] {SimpleType.Integer, SimpleType.Double});
      private static readonly object[][] _values = new object[][]
         {
            new object[] {new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               new TabularType("Name1", "Descr2", _compType1, new string[] { "Item1", "Item2"} ), 
               true},            
            new object[] {new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               new TabularType("Name2", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               false},
            new object[] {new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               new TabularType("Name1", "Descr1", _compType2, new string[] { "Item1", "Item2"} ), 
               false},
            new object[] {new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               new TabularType("Name1", "Descr1", _compType1, new string[] { "Item2", "Item1"} ), 
               false},
            new object[] {new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1", "Item2"} ), 
               new TabularType("Name1", "Descr1", _compType1, new string[] { "Item1"} ), 
               false},
         };

      [TestMethod]
      public void TestEquals()
      {
         for (int i = 0; i < _values.Length; i++)
         {
            TabularType left = (TabularType)_values[i][0];
            TabularType right = (TabularType)_values[i][1];
            bool result = (bool)_values[i][2];
            Assert.AreEqual(result, left.Equals(right));
            Assert.AreEqual(result, right.Equals(left));
         }
      }
      [TestMethod]
      public void TestEqualityOperator()
      {
         for (int i = 0; i < _values.Length; i++)
         {
            TabularType left = (TabularType)_values[i][0];
            TabularType right = (TabularType)_values[i][1];
            bool result = (bool)_values[i][2];
            Assert.AreEqual(result, left == right);
            Assert.AreEqual(result, right == left);
         }
      }
   }
}
