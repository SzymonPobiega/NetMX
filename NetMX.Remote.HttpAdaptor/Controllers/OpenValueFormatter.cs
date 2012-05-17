using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using NetMX.OpenMBean;

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
                return ExtractCompositeValue((CompositeType)openType, (Dictionary<string, string>)value);
            }
            if (openType.Kind == OpenTypeKind.ArrayType)
            {
                return ExtractArrayValue((ArrayType)openType, (string[])value);
            }
            if (openType.Kind == OpenTypeKind.TabularType)
            {
                return ExtractTabularValue((TabularType)openType, (Dictionary<string, string>[])value);
            }
            throw new NotSupportedException(string.Format("Open type kind {0} is not supported", openType.Kind));
        }

        private static ITabularData ExtractTabularValue(TabularType openType, Dictionary<string, string>[] value)
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

        private static ICompositeData ExtractCompositeValue(CompositeType openType, Dictionary<string, string> value)
        {
            var values = value.Keys.ToDictionary(x => x, x => ExtractSimpleValue(openType.GetOpenType(x), value[x]));
            return new CompositeDataSupport(openType, values);
        }

        private static object ExtractSimpleValue(OpenType openType, object value)
        {
            try
            {
                var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
                return typeConverter.ConvertFromString(null, CultureInfo.InvariantCulture, value.ToString());
            }
            catch (Exception)
            {
                throw new HttpResponseException(string.Format("Value {0} is not convertible to {1}", value, openType.Representation.Name), HttpStatusCode.BadRequest);
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

        private static Dictionary<string, string> FormatCompositeValue(CompositeType compositeType, ICompositeData compositeData)
        {
            return compositeType.KeySet
                .ToDictionary(x => x,
                              x => FormatSimpleValue(compositeType.GetOpenType(x), compositeData[x]));
        }

        private static string FormatSimpleValue(OpenType openType, object value)
        {
            var typeConverter = TypeDescriptor.GetConverter(openType.Representation);
            return typeConverter.ConvertToString(null, CultureInfo.InvariantCulture, value);
        }
    }
}