using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple= false)]
	public sealed class MBeanResourceAttribute : Attribute
	{
		private string _resourceName;
		/// <summary>
		/// Name of bound resource.
		/// </summary>
		public string ResourceName
		{
			get { return _resourceName; }
		}
		/// <summary>
		/// Creates new <see cref="NetMX.MBeanResourceAttribute"/> object.
		/// </summary>
		/// <param name="resourceName">Name of bound resource.</param>
		public MBeanResourceAttribute(string resourceName)
		{
			_resourceName = resourceName;
		}
	}
}
