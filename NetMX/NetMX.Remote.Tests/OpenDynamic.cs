using System;
using System.Collections.Generic;
using System.Linq;
using NetMX.OpenMBean;

namespace NetMX.Remote.Tests
{
   public class OpenDynamic : IDynamicMBean
   {
      private readonly CompositeType _compositeValueType;
      private readonly TabularType _innerTabularType;
      private readonly CompositeType _nestedCompositeValueType;
      private readonly TabularType _outerTabularType;
      private readonly TabularType _tabularType;
      private ITabularData _nestedTabularValue;
      private ITabularData _tabularValue;
      private readonly Func<MBeanInfo> _beanInfoGetter;

      public OpenDynamic()
      {
         var rowType = new CompositeType("Row", "Row", new[] {"ID", "Name"}, new[] {"Unique ID", "Name"},
                                         new[] {SimpleType.Integer, SimpleType.String});
         _tabularType = new TabularType("Table", "Table", rowType, new[] {"ID"});
         _tabularValue = new TabularDataSupport(_tabularType);

         _nestedCompositeValueType = new CompositeType("Nested composite value", "Nested composite value",
                                                       new[] {"NestedItem1", "NestedItem2"},
                                                       new[] {"Nested item 1", "Nested item 2"},
                                                       new[] {SimpleType.String, SimpleType.Double});
         _compositeValueType = new CompositeType("Composite value", "Composite value",
                                                 new[] {"Item1", "Item2", "Item3"},
                                                 new[] {"Item 1", "Item 2", "Item 3"},
                                                 new[]
                                                    {SimpleType.Integer, SimpleType.Boolean, _nestedCompositeValueType});

         var innerRowType = new CompositeType("Row", "Row", new[] {"ID", "Name", "CompositeValue"},
                                              new[] {"Unique ID", "Name", "Composite Value"},
                                              new[] {SimpleType.Integer, SimpleType.String, _compositeValueType});
         _innerTabularType = new TabularType("Inner table", "Inner table", innerRowType, new[] {"ID"});
         var outerRowType = new CompositeType("Outer Row", "Outer Row", new[] {"ID", "Value"},
                                              new[] {"Unique ID", "Tabular value"},
                                              new[] {SimpleType.Integer, _innerTabularType});
         _outerTabularType = new TabularType("Outer table", "Outer table", outerRowType, new[] {"ID"});
         _nestedTabularValue = new TabularDataSupport(_outerTabularType);

         _beanInfoGetter = MBean.Info<OpenDynamic>("Sample dynamic MBean")
            .WithAttributes(
               MBean.WritableAttribute("Attribute", "Sample attribute").TypedAs(TabularType),
               MBean.WritableAttribute("NestedTableAttribute", "Nested Table Attribute").TypedAs(NestedTabularType)
            )
            .WithOperations(
               MBean.MutatorOperation("DoSomething", "Does somthing").WithParameters(
                  MBean.Parameter("First", "First parameter").TypedAs(SimpleType.Double),
                  MBean.Parameter("Second", "Second parameter").TypedAs(TabularType)
               ).Returning(SimpleType.Void)               
            )            
            .AndNothingElse();    
      }

      public TabularType TabularType
      {
         get { return _tabularType; }
      }

      public TabularType NestedTabularType
      {
         get { return _outerTabularType; }
      }

      public TabularType InnerTabularType
      {
         get { return _innerTabularType; }
      }

      #region IDynamicMBean Members

      public MBeanInfo GetMBeanInfo()
      {
         return _beanInfoGetter();
      }

      public object GetAttribute(string attributeName)
      {
         switch (attributeName)
         {
            case "Attribute":
               return _tabularValue;
            case "NestedTableAttribute":
               return _nestedTabularValue;
            default:
               throw new NotSupportedException();
         }
      }

      public void SetAttribute(string attributeName, object value)
      {
         switch (attributeName)
         {
            case "Attribute":
               _tabularValue = (ITabularData) value;
               break;
            case "NestedTableAttribute":
               _nestedTabularValue = (ITabularData) value;
               break;
            default:
               throw new NotSupportedException();
         }
      }

      public object Invoke(string operationName, object[] arguments)
      {
         return null;
      }

      #endregion

      public void AddRow(int id, string name)
      {
         _tabularValue.Put(new CompositeDataSupport(_tabularType.RowType, new[] {"ID", "Name"}, new object[] {id, name}));
      }

      public void AddNestedRow(int outerId, int innerId, string name)
      {
         var nestedCompositeValue = new CompositeDataSupport(_nestedCompositeValueType,
                                                             new[] {"NestedItem1", "NestedItem2"},
                                                             new object[] {"1", 5.7});
         var compositeValue = new CompositeDataSupport(_compositeValueType, new[] {"Item1", "Item2", "Item3"},
                                                       new object[] {1, false, nestedCompositeValue});
         var innerRow = new CompositeDataSupport(_innerTabularType.RowType, new[] {"ID", "Name", "CompositeValue"},
                                                 new object[] {outerId, name, compositeValue});
         var innerTable = new TabularDataSupport(_innerTabularType);
         innerTable.Put(innerRow);
         var outerRow = new CompositeDataSupport(_outerTabularType.RowType, new[] {"ID", "Value"},
                                                 new object[] {innerId, innerTable});
         _nestedTabularValue.Put(outerRow);
      }
   }
}