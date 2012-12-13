using System;
using System.IO;
using System.Linq;
using NetMX.Remote.HttpAdaptor.Resources;

namespace NetMX.Remote.HttpAdaptor.Formatters
{
    public class MBeanAttributeHtmlFormatter : HtmlFormatterBase
    {
        public override bool CanWriteType(System.Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }

        protected override void WriteBody(object value, TextWriter writer)
        {
            var typedValue = (MBeanAttributeResource)value;
            writer.WriteLine("<div>");
            writer.WriteLine("<div>");
            SerializeValue(typedValue.Value, writer);
            writer.WriteLine("</div>");
            writer.WriteLine(string.Format("<a href=\"{0}\">MBean</a>",typedValue.MBeanHRef));
            writer.WriteLine("</div>");
        }

        protected override string Title
        {
            get { return "MBean attribute"; }
        }

        private static void SerializeValue(object value, TextWriter writer)
        {
            var stringValue = value as String;
            if (stringValue != null)
            {
                writer.Write(value);
                return;
            }
            var arrayValue = value as string[];
            if (arrayValue != null)
            {                
                SerializeArrayValue(writer, arrayValue);
                return;
            }
            var compositeValue = value as CompositeData;
            if (compositeValue != null)
            {
                SerializeCompositeValue(writer, compositeValue);
                return;
            }
            var tabularValue = value as CompositeData[];
            if (tabularValue != null)
            {
                SerializeTabularValue(writer, tabularValue);
                return;
            }
            throw new NotSupportedException("Not supported value type: " + value.GetType().FullName);
        }

        private static void SerializeTabularValue(TextWriter writer, CompositeData[] tabularValue)
        {
            var firstRow = tabularValue.First();
            writer.WriteLine("<table>");
            writer.WriteLine("<tr>");
            foreach (var column in firstRow.Properties)
            {
                writer.WriteLine("<th>{0}</th>", column.Name);
            }
            writer.WriteLine("</tr");
            foreach (var row in tabularValue)
            {
                writer.WriteLine("<tr>");
                foreach (var column in row.Properties)
                {
                    writer.WriteLine("<td>{0}</td>", column.Value);
                }
                writer.WriteLine("</tr>");
            }
            writer.WriteLine("</table>");
        }

        private static void SerializeCompositeValue(TextWriter writer, CompositeData compositeValue)
        {
            writer.WriteLine("<ul>");
            foreach (var property in compositeValue.Properties)
            {
                writer.WriteLine("<li><span>{0}</span> : <span>{1}</span></li>", property.Name, property.Value);
            }
            writer.WriteLine("</ul>");
        }

        private static void SerializeArrayValue(TextWriter writer, string[] arrayValue)
        {
            writer.WriteLine("<ol>");
            foreach (var element in arrayValue)
            {
                writer.WriteLine("<li>{0}</li>", element);
            }
            writer.WriteLine("</ol>");
        }
    }
}