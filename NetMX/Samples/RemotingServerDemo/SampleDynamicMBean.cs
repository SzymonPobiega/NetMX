#region Using
using System;
using NetMX;
using NetMX.OpenMBean;

#endregion

namespace RemotingServerDemo
{
   public class SampleDynamicMBean : IDynamicMBean
   {
      private ITabularData _tabularValue;
      private ITabularData _nestedTabularValue;

      private readonly TabularType _tabularType;
      private readonly TabularType _outerTabularType;
      private readonly TabularType _innerTabularType;
      private readonly CompositeType _compositeValueType;
      private readonly CompositeType _nestedCompositeValueType;

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

      public SampleDynamicMBean()
      {
         CompositeType rowType = new CompositeType("Row", "Row", new string[] { "ID", "Name" }, new string[] { "Unique ID", "Name" }, new OpenType[] { SimpleType.Integer, SimpleType.String });
         _tabularType = new TabularType("Table", "Table", rowType, new string[] { "ID" });
         _tabularValue = new TabularDataSupport(_tabularType);

         _nestedCompositeValueType = new CompositeType("Nested composite value", "Nested composite value",
                                                 new string[] {"NestedItem1", "NestedItem2"},
                                                 new string[] {"Nested item 1", "Nested item 2"},
                                                 new OpenType[] {SimpleType.String, SimpleType.Double});
         _compositeValueType = new CompositeType("Composite value", "Composite value", 
                                                 new string[] { "Item1", "Item2", "Item3" }, 
                                                 new string[] { "Item 1", "Item 2", "Item 3" }, 
                                                 new OpenType[] { SimpleType.Integer, SimpleType.Boolean, _nestedCompositeValueType });         

         CompositeType innerRowType = new CompositeType("Row", "Row", new string[] { "ID", "Name", "CompositeValue" }, new string[] { "Unique ID", "Name", "Composite Value" }, new OpenType[] { SimpleType.Integer, SimpleType.String, _compositeValueType });
         _innerTabularType = new TabularType("Inner table", "Inner table", innerRowType, new string[] { "ID" });
         CompositeType outerRowType = new CompositeType("Outer Row", "Outer Row", new string[] { "ID", "Value" }, new string[] { "Unique ID", "Tabular value" }, new OpenType[] { SimpleType.Integer, _innerTabularType });
         _outerTabularType = new TabularType("Outer table", "Outer table", outerRowType, new string[] { "ID" });
         _nestedTabularValue = new TabularDataSupport(_outerTabularType);
      }

      public void AddRow(int id, string name)
      {
         _tabularValue.Put(new CompositeDataSupport(_tabularType.RowType, new string[] { "ID", "Name" }, new object[] { id, name }));
      }

      public void AddNestedRow(int outerId, int innerId, string name)
      {
         CompositeDataSupport nestedCompositeValue = new CompositeDataSupport(_nestedCompositeValueType, new string[] { "NestedItem1", "NestedItem2" }, new object[] { "1", 5.7 });
         CompositeDataSupport compositeValue = new CompositeDataSupport(_compositeValueType, new string[] { "Item1", "Item2", "Item3" }, new object[] { 1, false, nestedCompositeValue });
         CompositeDataSupport innerRow = new CompositeDataSupport(_innerTabularType.RowType, new string[] { "ID", "Name", "CompositeValue" }, new object[] { outerId, name, compositeValue });
         TabularDataSupport innerTable = new TabularDataSupport(_innerTabularType);
         innerTable.Put(innerRow);
         CompositeDataSupport outerRow = new CompositeDataSupport(_outerTabularType.RowType, new string[] { "ID", "Value" }, new object[] { innerId, innerTable });
         _nestedTabularValue.Put(outerRow);
      }

      #region IDynamicMBean Members
      public MBeanInfo GetMBeanInfo()
      {
         IOpenMBeanAttributeInfo[] attributes = new IOpenMBeanAttributeInfo[]
            {
               new OpenMBeanAttributeInfoSupport("Attribute", "Sample attribute", TabularType, true, true),
               new OpenMBeanAttributeInfoSupport("NestedTableAttribute", "Nested Table Attribute", NestedTabularType, true, true)
            };
         IOpenMBeanConstructorInfo[] constructors = new IOpenMBeanConstructorInfo[] { };
         IOpenMBeanOperationInfo[] operations = new IOpenMBeanOperationInfo[] { };
         MBeanNotificationInfo[] notifs = new MBeanNotificationInfo[] { };
         OpenMBeanInfoSupport info =
            new OpenMBeanInfoSupport(typeof(SampleDynamicMBean).AssemblyQualifiedName, "Sample dynamic MBean",
                                     attributes, constructors, operations, notifs);
         return info;
      }
      public object GetAttribute(string attributeName)
      {
         switch (attributeName)
         {
            case "Attribute":
               return _tabularValue;
            case "NestedTableAttribute":
               return _nestedTabularValue;
            default: throw new NotSupportedException();

         }
      }
      public void SetAttribute(string attributeName, object value)
      {
         switch (attributeName)
         {
            case "Attribute":
               _tabularValue = (ITabularData)value;
               break;
            case "NestedTableAttribute":
               _nestedTabularValue = (ITabularData)value;
               break;
            default: throw new NotSupportedException();

         }
      }
      public object Invoke(string operationName, object[] arguments)
      {
         return null;
      }
      #endregion
   }
}