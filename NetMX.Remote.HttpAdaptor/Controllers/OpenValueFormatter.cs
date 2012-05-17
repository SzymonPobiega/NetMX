using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Http;
using NetMX.OpenMBean;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class OpenValueFormatter
    {
        public static object ExtractValue(OpenType openType, object value)
        {            
            if (openType.Kind == OpenTypeKind.SimpleType)
            {
                return ExtractSimpleValue(openType, value);
            }
            if (openType.Kind == OpenTypeKind.CompositeType)
            {
                return ExtractCompositeValue((CompositeType)openType, (CompositeData)value);
            }
            if (openType.Kind == OpenTypeKind.ArrayType)
            {
                return ExtractArrayValue((ArrayType)openType, (string[])value);
            }
            if (openType.Kind == OpenTypeKind.TabularType)
            {
                return ExtractTabularValue((TabularType)openType, (CompositeData[])value);
            }
            throw new NotSupportedException(string.Format("Open type kind {0} is not supported", openType.Kind));
        }

        private static ITabularData ExtractTabularValue(TabularType openType, CompositeData[] value)
        {
            var tabularValue = new TabularDataSupport(openType);
            tabularValue.PutAll(value.Select(x => ExtractCompositeValue(openType.RowType, x)));
            return tabularValue;
        }

        private static Array ExtractArrayValue(ArrayType openType, string[] value)
        {
            var objectArray = value.Select(x => ExtractSimpleValue(openType.ElementType, x)).ToArray();
            var typedArray = Array.CreateInstance(openType.ElementType.Representation, objectArray.Length);
            Array.Copy(objectArray, typedArray, objectArray.Length);
            return typedArray;
        }

        private static ICompositeData ExtractCompositeValue(CompositeType openType, CompositeData compositeData)
        {
            return new CompositeDataSupport(openType, 
                compositeData.Properties.Select(x => x.Name), 
                compositeData.Properties.Select(x => ExtractSimpleValue(openType.GetOpenType(x.Name), x.Value)));
        }

        private static object ExtractSimpleValue(OpenType openType, object value)
        {
            try
            {
                var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
                // ReSharper disable PossibleNullReferenceException
                return typeConverter.ConvertFromInvariantString(value.ToString());
                // ReSharper restore PossibleNullReferenceException              
            }
            catch (Exception)
            {
                throw new FormatException(string.Format("Value {0} is not convertible to {1}", value, openType.Representation.Name));
            }
        }

        public static object FormatValue(OpenType openType, object value)
        {
            if (openType.Kind == OpenTypeKind.SimpleType)
            {
                return FormatSimpleValue(openType, value);
            }
            if (openType.Kind == OpenTypeKind.TabularType)
            {
                var tabularValue = (ITabularData)value;
                var tabularType = (TabularType)openType;
                return tabularValue.Values.Select(x => FormatCompositeValue(tabularType.RowType, x)).ToArray();
            }
            if (openType.Kind == OpenTypeKind.CompositeType)
            {
                var compositeValue = (ICompositeData)value;
                var compositeType = (CompositeType)openType;
                return FormatCompositeValue(compositeType, compositeValue);
            }
            if (openType.Kind == OpenTypeKind.ArrayType)
            {
                var arrayType = (ArrayType)openType;
                var arrayValue = (IEnumerable)value;
                return arrayValue.Cast<object>().Select(x => FormatSimpleValue(arrayType.ElementType, x)).ToArray();
            }
            throw new NotSupportedException(string.Format("Open type kind {0} is not supported", openType.Kind));
        }

        private static CompositeData FormatCompositeValue(CompositeType compositeType, ICompositeData compositeData)
        {
            return new CompositeData(compositeType.KeySet.Select(x => new CompositeDataProperty(x, FormatSimpleValue(compositeType.GetOpenType(x), compositeData[x]))));
        }

        private static string FormatSimpleValue(OpenType openType, object value)
        {
            var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
// ReSharper disable PossibleNullReferenceException
            return typeConverter.ConvertToInvariantString(value);
// ReSharper restore PossibleNullReferenceException
        }
    }
}