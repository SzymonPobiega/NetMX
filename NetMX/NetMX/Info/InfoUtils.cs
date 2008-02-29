using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace NetMX
{
    internal static class InfoUtils
    {
        internal static string GetDescrition(ICustomAttributeProvider provider, string defaultValue)
        {
            object[] attributes = provider.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes.Length > 0)
            {
                return ((DescriptionAttribute)attributes[0]).Description;
            }
            return defaultValue;
        }
    }
}
