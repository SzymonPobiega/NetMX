using System;
using NetMX.OpenMBean;

namespace NetMX.Remote.HttpAdaptor.Tests
{
    public class SampleDynamicMBean : IDynamicMBean
    {
        private readonly TabularType _tabularType;
        private ITabularData _tabularValue;
        private readonly CompositeType _rowType;
        private ICompositeData _compositeValue;
        private readonly ArrayType _arrayType;
        private decimal[] _arrayValue;

        public SampleDynamicMBean()
        {
            _rowType = new CompositeType("Row", "Row", new[] { "ID", "Name" }, new[] { "Unique ID", "Name" },
                                         new[] { SimpleType.Integer, SimpleType.String });
            _tabularType = new TabularType("Table", "Table", _rowType, new[] { "ID" });
            _tabularValue = new TabularDataSupport(_tabularType);
            _arrayType = new ArrayType(1, SimpleType.Decimal);
        }


        public MBeanInfo GetMBeanInfo()
        {
            return MBean.Info<SampleDynamicMBean>("Sample dynamic MBean")
                .WithAttributes(
                    MBean.WritableAttribute("Tabular", "Sample tabular attribute").TypedAs(_tabularType),
                    MBean.WritableAttribute("Array", "Sample array attribute").TypedAs(_arrayType),
                    MBean.WritableAttribute("Composite", "Sample composite attribute").TypedAs(_rowType)
                )
                .AndNothingElse()();
        }

        public object GetAttribute(string attributeName)
        {
            switch (attributeName)
            {
                case "Tabular":
                    return _tabularValue;
                case "Array":
                    return _arrayValue;
                case "Composite":
                    return _compositeValue;
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetAttribute(string attributeName, object value)
        {
            switch (attributeName)
            {
                case "Tabular":
                    _tabularValue = (ITabularData)value;
                    break;
                case "Array":
                    _arrayValue = (decimal[]) value;
                    break;                    
                case "Composite":
                    _compositeValue = (ICompositeData) value;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public object Invoke(string operationName, object[] arguments)
        {
            return null;
        }        

        public void SetComposite(int id, string name)
        {
            _compositeValue = new CompositeDataSupport(_tabularType.RowType, new[] { "ID", "Name" }, new object[] { id, name });
        }

        public void SetArray(decimal[] array)
        {
            _arrayValue = array;
        }

        public void AddRow(int id, string name)
        {
            _tabularValue.Put(new CompositeDataSupport(_tabularType.RowType, new[] { "ID", "Name" }, new object[] { id, name }));
        }
    }
}