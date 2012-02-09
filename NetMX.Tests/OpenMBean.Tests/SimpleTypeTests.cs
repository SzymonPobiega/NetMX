using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace NetMX.OpenMBean.Tests
{
   /// <summary>
   /// Summary description for UnitTest1
   /// </summary>
   [TestFixture]
   public class SimpleTypeTests
   {
      private readonly object[][] _values = new object[][]
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
 
      [Test]
      public void TestIsValue()
      {         
         for (int i = 0; i < _values.Length; i++)
         {
            for (int j = 0; j < _values.Length; j++)
            {
               SimpleType st = (SimpleType) _values[i][0];
               object value = _values[j][1];
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
      [Test]
      public void TestEquals()
      {
         for (int i = 0; i < _values.Length; i++)
         {
            for (int j = 0; j < _values.Length; j++)
            {
               SimpleType thisType = (SimpleType) _values[i][0];
               SimpleType otherType = (SimpleType) _values[j][0];
               if (i != j)
               {
                  Assert.IsFalse(thisType.Equals(otherType));
               }
               else
               {
                  Assert.IsTrue(thisType.Equals(otherType));
               }
            }
         }
      }
      [Test]
      public void TestEqualityOperator()
      {
         for (int i = 0; i < _values.Length; i++)
         {
            for (int j = 0; j < _values.Length; j++)
            {
               SimpleType thisType = (SimpleType)_values[i][0];
               SimpleType otherType = (SimpleType)_values[j][0];
               if (i != j)
               {
                  Assert.IsFalse(thisType == otherType);
               }
               else
               {
                  Assert.IsTrue(thisType == otherType);
               }
            }
         }
      }
   }
}

