using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetMX.OpenMBean.Mapper;
using NetMX.Server.OpenMBean.Mapper;
using NetMX.Server.OpenMBean.Mapper.TypeMappers;

namespace NetMX.OpenMBean.Mapper.Tests
{
   [TestClass]
   public class CollectionTypeMapperTest : MapperTestBase
   {
      private readonly ITypeMapper mapper = new CollectionTypeMapper();
      protected override ITypeMapper Mapper
      {
         get { return mapper; }
      }

      protected override bool CanHandle(Type plainNetType, out OpenTypeKind mapsTo)
      {
         if (plainNetType == typeof(TestCollectionElement))
         {
            mapsTo = OpenTypeKind.CompositeType;
            return true;
         }
         return base.CanHandle(plainNetType, out mapsTo);
      }

      protected override OpenType MapType(Type plainNetType)
      {
         if (plainNetType == typeof(TestCollectionElement))
         {
            return new CompositeType("TestCollectionElement", "TestCollectionElement",
                                     new string[] { "IntValue", "StringValue" }, new string[] { "IntValue", "StringValue" },
                                     new OpenType[] {SimpleType.Integer, SimpleType.String});
         }
         return base.MapType(plainNetType);
      }

      protected override object MapValue(Type clrType, OpenType mappedType, object value)
      {
         if (mappedType.TypeName == "TestCollectionElement")
         {
            return new CompositeDataSupport((CompositeType)mappedType, new string[] { "IntValue", "StringValue" },
                                            new object[] {((TestCollectionElement) value).IntValue,
                                            ((TestCollectionElement) value).StringValue});
         }
         return base.MapValue(clrType, mappedType, value);
      }

      #region Test type
      private class TestCollectionElement
      {
         private int _intValue;
         public int IntValue
         {
            get { return _intValue; }
         }
         private string _stringValue;
         public string StringValue
         {
            get { return _stringValue; }
         }
         public TestCollectionElement(int intValue, string stringValue)
         {
            _intValue = intValue;
            _stringValue = stringValue;
         }
      }
      #endregion

      [TestMethod]
      public void CollectionMapperCanHandleMappingIEnumerableToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof (IEnumerable<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingICollectionToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(ICollection<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingIListToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(IList<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingListToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(List<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingReadOnlyCollectionToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(ReadOnlyCollection<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingLinkedListToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(LinkedList<int>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingCollectionToArray()
      {
         OpenType mappedType = mapper.MapType(typeof (IEnumerable<int>), MapType);
         Assert.AreEqual(OpenTypeKind.ArrayType, mappedType.Kind);

         ArrayType arrayType = (ArrayType) mappedType;
         Assert.AreEqual(1, arrayType.Dimension);
         Assert.AreEqual(SimpleType.Integer, arrayType.ElementType);

         List<int> value = new List<int>();
         value.Add(1);
         value.Add(2);
         value.Add(3);

         object mappedValue = mapper.MapValue(typeof(IEnumerable<int>), mappedType, value, MapValue);
         Assert.IsTrue(mappedValue is int[]);
         int[] array = (int[]) mappedValue;
         Assert.AreEqual(3, array.Length);
         Assert.AreEqual(1, array[0]);
         Assert.AreEqual(2, array[1]);
         Assert.AreEqual(3, array[2]);
      }

      [TestMethod]
      public void CollectionMapperCanHandleMappingArrayToArray()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(int[]), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.ArrayType, mapsTo);

         OpenType mappedType = mapper.MapType(typeof(int[]), MapType);
         Assert.AreEqual(OpenTypeKind.ArrayType, mappedType.Kind);

         ArrayType arrayType = (ArrayType)mappedType;
         Assert.AreEqual(1, arrayType.Dimension);
         Assert.AreEqual(SimpleType.Integer, arrayType.ElementType);

         int[] value = new int[] {1, 2, 3};
         
         object mappedValue = mapper.MapValue(typeof(int[]), mappedType, value, MapValue);
         Assert.IsTrue(mappedValue is int[]);
         int[] array = (int[])mappedValue;
         Assert.AreEqual(3, array.Length);
         Assert.AreEqual(1, array[0]);
         Assert.AreEqual(2, array[1]);
         Assert.AreEqual(3, array[2]);
      }

      [TestMethod]
      public void CollectionMapperCanHandleMappingIEnumerableToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(IEnumerable<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingICollectionToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(ICollection<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingIListToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(IList<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingListToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(List<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingLinkedListToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(LinkedList<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }
      [TestMethod]
      public void CollectionMapperCanHandleMappingReadOnlyCollectionToTabular()
      {
         OpenTypeKind mapsTo;
         Assert.IsTrue(mapper.CanHandle(typeof(ReadOnlyCollection<TestCollectionElement>), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.TabularType, mapsTo);
      }

      [TestMethod]
      public void CollectionMapperCanHandleMappingCollectionToTabular()
      {
         OpenType mappedType = mapper.MapType(typeof(IEnumerable<TestCollectionElement>), MapType);
         Assert.AreEqual(OpenTypeKind.TabularType, mappedType.Kind);

         TabularType tabuarType = (TabularType)mappedType;
         Assert.AreEqual(tabuarType.IndexNames.Count, 1);
         Assert.IsTrue(tabuarType.IndexNames.Contains("CollectionIndex"));
         Assert.AreEqual(3, tabuarType.RowType.KeySet.Count);
         Assert.IsTrue(tabuarType.RowType.ContainsKey("IntValue"));
         Assert.IsTrue(tabuarType.RowType.ContainsKey("StringValue"));
         Assert.IsTrue(tabuarType.RowType.ContainsKey("CollectionIndex"));
         Assert.AreEqual(SimpleType.Integer, tabuarType.RowType.GetOpenType("IntValue"));
         Assert.AreEqual(SimpleType.String, tabuarType.RowType.GetOpenType("StringValue"));
         Assert.AreEqual(SimpleType.Integer, tabuarType.RowType.GetOpenType("CollectionIndex"));

         List<TestCollectionElement> value = new List<TestCollectionElement>();
         value.Add(new TestCollectionElement(1, "1"));
         value.Add(new TestCollectionElement(2, "2"));
         value.Add(new TestCollectionElement(3, "3"));         

         object mappedValue = mapper.MapValue(typeof(IEnumerable<TestCollectionElement>), mappedType, value, MapValue);
         Assert.IsTrue(mappedValue is ITabularData);
         ITabularData table = (ITabularData)mappedValue;
         Assert.AreEqual(3, table.Count);
         ICompositeData el = table[new object[] { 0 }];
         Assert.AreEqual(1, el["IntValue"]);
         Assert.AreEqual("1", el["StringValue"]);
         el = table[new object[] { 1 }];
         Assert.AreEqual(2, el["IntValue"]);
         Assert.AreEqual("2", el["StringValue"]);
         el = table[new object[] { 2 }];
         Assert.AreEqual(3, el["IntValue"]);
         Assert.AreEqual("3", el["StringValue"]);
      }
   }
}
