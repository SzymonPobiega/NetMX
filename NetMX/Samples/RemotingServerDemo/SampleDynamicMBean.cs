#region Using
using System;
using NetMX;
using NetMX.OpenMBean;

#endregion

namespace RemotingServerDemo
{
   public class SampleDynamicMBean : IDynamicMBean
   {
      private ITabularData _value;
      private TabularType _type;
      public TabularType Type
      {
         get { return _type; }
      }

      public SampleDynamicMBean()
      {
         CompositeType rowType = new CompositeType("Row", "Row", new string[] { "ID", "Name" }, new string[] { "Unique ID", "Name" }, new OpenType[] { SimpleType.Integer, SimpleType.String });
         _type = new TabularType("Table", "Table", rowType, new string[] { "ID" });
         _value = new TabularDataSupport(_type);
      }      

      public void AddRow(int id, string name)
      {
         _value.Put(new CompositeDataSupport(_type.RowType, new string[] {"ID", "Name"}, new object[] {id, name}));
      }

      #region IDynamicMBean Members
      public MBeanInfo GetMBeanInfo()
      {         
         IOpenMBeanAttributeInfo[] attributes = new IOpenMBeanAttributeInfo[]
            {
               new OpenMBeanAttributeInfoSupport("Attribute", "Sample attribute", Type, true, true)
            };
         IOpenMBeanConstructorInfo[] constructors = new IOpenMBeanConstructorInfo[] {};
         IOpenMBeanOperationInfo[] operations = new IOpenMBeanOperationInfo[] {};
         MBeanNotificationInfo[] notifs = new MBeanNotificationInfo[] {};
         OpenMBeanInfoSupport info =
            new OpenMBeanInfoSupport(typeof (SampleDynamicMBean).AssemblyQualifiedName, "Sample dynamic MBean",
                                     attributes, constructors, operations, notifs);
         return info;
      }
      public object GetAttribute(string attributeName)
      {
         return _value;
      }
      public void SetAttribute(string attributeName, object value)
      {
         _value = (ITabularData) value;
      }
      public object Invoke(string operationName, object[] arguments)
      {
         return null;
      }
      #endregion
   }
}