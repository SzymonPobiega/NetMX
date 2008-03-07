using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Resources;

namespace NetMX
{
	internal static class InfoUtils
	{
		internal static string GetDescrition(MemberInfo member, ICustomAttributeProvider provider, string defaultValue)
		{
			Type t = member as Type;
			if (t == null)
			{
				t = member.DeclaringType;
			}
			object[] attributes = t.GetCustomAttributes(typeof(MBeanResourceAttribute), true);
			if (attributes.Length > 0)
			{				
				ResourceManager manager = new ResourceManager(((MBeanResourceAttribute)attributes[0]).ResourceName, t.Assembly);
				string descr = manager.GetString(member.Name);
				if (descr != null)
				{
					return descr;
				}
			}
			attributes = provider.GetCustomAttributes(typeof(DescriptionAttribute), true);
			if (attributes.Length > 0)
			{
				return ((DescriptionAttribute)attributes[0]).Description;
			}
			return defaultValue;
		}
	}
}
