using System;
using System.Collections.Generic;
using System.Linq;
using NetMX.OpenMBean;

namespace NetMX.Remote.Tests
{
   public class OpenDynamic : IDynamicMBean
   {
      private readonly TabularType _outerTabularType;
      private readonly TabularType _tabularType;
      private ITabularData _nestedTabularValue;
      private ITabularData _tabularValue;
      private readonly Func<MBeanInfo> _beanInfoGetter;

      public OpenDynamic()
      {
         _tabularType = Tabular.Type("Table", "Table")
            .WithIndexColumn("ID", "UniqueID").TypedAs(SimpleType.Integer)
            .WithColumn("Name", "Name").TypedAs(SimpleType.String);
         
         _tabularValue = new TabularDataSupport(_tabularType);

         _outerTabularType = Tabular.Type("Outer table", "Outer table")
            .WithIndexColumn("ID", "Unique ID").TypedAs(SimpleType.Integer)
            .WithColumn("Value", "Tabular value").TypedAs(
               Tabular.Type("Inner table", "Inner table")
                  .WithIndexColumn("ID", "Unique ID").TypedAs(SimpleType.Integer)
                  .WithColumn("Name", "Name").TypedAs(SimpleType.String)
                  .WithColumn("CompositeValue", "Composite Value").TypedAs(
                     Composite.Type("Composite value", "Composite value")
                        .WithItem("Item1", "Item 2").TypedAs(SimpleType.Integer)
                        .WithItem("Item2", "Item 2").TypedAs(SimpleType.Boolean)
                        .WithItem("Item3", "Item 3").TypedAs(
                           Composite.Type("Nested", "Nested composite")
                              .WithItem("NestedItem1", "NestedItem1").TypedAs(SimpleType.String)
                              .WithItem("NestedItem2", "NestedItem2").TypedAs(SimpleType.Double))));

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
         _tabularValue.Put(x =>
                              {
                                 x.Simple("ID", id);
                                 x.Simple("Name", name);
                              });
      }

      public void AddNestedRow(int outerId, int innerId, string name)
      {
         _nestedTabularValue.Put(r =>
                                    {
                                       r.Simple("ID", innerId);
                                       r.Table("Value",
                                               t => t.Put(x =>
                                                             {
                                                                x.Simple("ID", outerId);
                                                                x.Simple("Name", name);
                                                                x.Composite("CompositeValue",
                                                                            y =>
                                                                               {
                                                                                  y.Simple("Item1", 1);
                                                                                  y.Simple("Item2", false);
                                                                                  y.Composite("Item3",
                                                                                              z =>
                                                                                                 {
                                                                                                    z.Simple(
                                                                                                       "NestedItem1",
                                                                                                       "1");
                                                                                                    z.Simple(
                                                                                                       "NestedItem2",
                                                                                                       5.7);
                                                                                                 });
                                                                               });
                                                             }));
                                    });
      }
   }
}