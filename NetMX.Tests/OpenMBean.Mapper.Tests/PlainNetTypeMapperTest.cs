using System;
using System.Linq;
using NUnit.Framework;
using NetMX.OpenMBean.Mapper;
using NetMX.Server.OpenMBean.Mapper;
using NetMX.Server.OpenMBean.Mapper.Attributes;
using NetMX.Server.OpenMBean.Mapper.TypeMappers;

namespace NetMX.OpenMBean.Mapper.Tests
{     
   [TestFixture]
   public class PlainNetTypeMapperTest : MapperTestBase
   {
      private ITypeMapper mapper = new PlainNetTypeMapper();
      protected override ITypeMapper Mapper
      {
         get { return mapper; }
      }
      
      #region Test type
      public class SimpleFlatType
      {
         private int _intValue;
         public int IntValue
         {
            get { return _intValue; }
            set { _intValue = value; }
         }
         private string _stringValue;
         public string StringValue
         {
            get { return _stringValue; }
            set { _stringValue = value; }
         }
      }
      #endregion

      [Test]
      public void PlainNetMapperCanHandleSimpleFlatTypes()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(SimpleFlatType), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.CompositeType, mapsTo);         

         OpenType mappedType = mapper.MapType(typeof (SimpleFlatType), MapType);

         Assert.AreEqual(OpenTypeKind.CompositeType, mappedType.Kind);

         CompositeType compositeType = (CompositeType) mappedType;
         
         Assert.AreEqual("SimpleFlatType", compositeType.TypeName);
         Assert.AreEqual("SimpleFlatType", compositeType.Description);
         Assert.IsTrue(compositeType.ContainsKey("IntValue"));
         Assert.IsTrue(compositeType.ContainsKey("StringValue"));
         Assert.AreEqual(2, compositeType.KeySet.Count);
         Assert.AreEqual(SimpleType.Integer, compositeType.GetOpenType("IntValue"));
         Assert.AreEqual(SimpleType.String, compositeType.GetOpenType("StringValue"));

         SimpleFlatType value = new SimpleFlatType();
         value.IntValue = 5;
         value.StringValue = "A text";

         object mappedValue = mapper.MapValue(typeof(SimpleFlatType), mappedType, value, MapValue);
         Assert.IsTrue(mappedValue is ICompositeData);
         ICompositeData compositeData = (ICompositeData) mappedValue;
         Assert.IsTrue(compositeType.IsValue(compositeData));
         Assert.IsTrue(compositeData.ContainsKey("IntValue"));
         Assert.IsTrue(compositeData.ContainsKey("StringValue"));
         Assert.AreEqual(value.IntValue, compositeData["IntValue"]);
         Assert.AreEqual(value.StringValue, compositeData["StringValue"]);
         Assert.AreEqual(2, compositeData.Values.Count());
      }

      #region Test types
      private class InnerType
      {
         private string _value;
         public string Value
         {
            get { return _value; }
            set { _value = value; }
         }
      }

      private class OuterType
      {
         private InnerType _inner;
         public InnerType Inner
         {
            get { return _inner; }
            set { _inner = value; }
         }
      }
      #endregion

      [Test]
      public void PlainNetMapperCanHandleComplexTypes()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(OuterType), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.CompositeType, mapsTo);

         OpenType outerType = mapper.MapType(typeof(OuterType), MapType);

         Assert.AreEqual(OpenTypeKind.CompositeType, outerType.Kind);

         CompositeType outerCompositeType = (CompositeType)outerType;

         Assert.AreEqual("OuterType", outerCompositeType.TypeName);
         Assert.AreEqual("OuterType", outerCompositeType.Description);
         Assert.IsTrue(outerCompositeType.ContainsKey("Inner"));         
         Assert.AreEqual(1, outerCompositeType.KeySet.Count);

         OpenType innerType = outerCompositeType.GetOpenType("Inner");
         Assert.AreEqual(OpenTypeKind.CompositeType, innerType.Kind);

         CompositeType innerCompositeType = (CompositeType) innerType;
         Assert.AreEqual("InnerType", innerCompositeType.TypeName);
         Assert.AreEqual("InnerType", innerCompositeType.Description);
         Assert.IsTrue(innerCompositeType.ContainsKey("Value"));
         Assert.AreEqual(SimpleType.String, innerCompositeType.GetOpenType("Value"));
         Assert.AreEqual(1, outerCompositeType.KeySet.Count);

         OuterType value = new OuterType();
         value.Inner = new InnerType();
         value.Inner.Value = "Inner text value";

         object mappedValue = mapper.MapValue(typeof(OuterType), outerType, value, MapValue);
         Assert.IsTrue(mappedValue is ICompositeData);
         ICompositeData outerCompositeData = (ICompositeData)mappedValue;
         Assert.IsTrue(outerCompositeType.IsValue(outerCompositeData));
         Assert.IsTrue(outerCompositeData.ContainsKey("Inner"));
         Assert.AreEqual(1, outerCompositeData.Values.Count());         
         Assert.IsTrue(outerCompositeData["Inner"] is ICompositeData);
         ICompositeData innerCompositeData = (ICompositeData) outerCompositeData["Inner"];
         Assert.IsTrue(innerCompositeData.ContainsKey("Value"));
         Assert.AreEqual(1, innerCompositeData.Values.Count());
         Assert.AreEqual(value.Inner.Value, innerCompositeData["Value"]);
      }

      #region Test type
      [OpenType(ResourceName = "NetMX.OpenMBean.Mapper.Tests.ResourceUsageTest")]
      private class ResourceTestType
      {
         private int _intValue;
         public int IntValue
         {
            get { return _intValue; }
            set { _intValue = value; }
         }
         private string _stringValue;
         public string StringValue
         {
            get { return _stringValue; }
            set { _stringValue = value; }
         }
      }
      #endregion

      [Test]
      public void MappersUseDescriptionsFromResourceFile()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(ResourceTestType), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.CompositeType, mapsTo);

         OpenType mappedType = mapper.MapType(typeof(ResourceTestType), MapType);

         Assert.AreEqual(OpenTypeKind.CompositeType, mappedType.Kind);

         CompositeType compositeType = (CompositeType)mappedType;

         Assert.AreEqual(ResourceUsageTest.ResourceTestType, compositeType.Description);
         Assert.IsTrue(compositeType.ContainsKey("IntValue"));
         Assert.IsTrue(compositeType.ContainsKey("StringValue"));
         Assert.AreEqual(ResourceUsageTest.IntValue, compositeType.GetDescription("IntValue"));
         Assert.AreEqual(ResourceUsageTest.StringValue, compositeType.GetDescription("StringValue"));
      }

      #region Test type
      [OpenType(MappedName = "CustomizedMappedType" )]
      private class MappedNameTestType
      {
         private int _intValue;
         public int IntValue
         {
            get { return _intValue; }
            set { _intValue = value; }
         }
         private string _stringValue;
         public string StringValue
         {
            get { return _stringValue; }
            set { _stringValue = value; }
         }
      }
      #endregion

      [Test]
      public void MappersUseCustomizedMappedTypeName()
      {
         OpenTypeKind mapsTo;

         Assert.IsTrue(mapper.CanHandle(typeof(MappedNameTestType), out mapsTo, CanHandle));
         Assert.AreEqual(OpenTypeKind.CompositeType, mapsTo);

         OpenType mappedType = mapper.MapType(typeof(MappedNameTestType), MapType);

         Assert.AreEqual(OpenTypeKind.CompositeType, mappedType.Kind);

         CompositeType compositeType = (CompositeType)mappedType;

         Assert.AreEqual("CustomizedMappedType", compositeType.TypeName);
      }
   }
}
