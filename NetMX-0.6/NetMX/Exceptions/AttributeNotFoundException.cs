#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Other constructos do not make sense"), Serializable]
	public sealed class AttributeNotFoundException : OperationsException
	{
		private readonly string _attributeName;
		/// <summary>
		/// Missing attribute name.
		/// </summary>
		public string AttributeName
		{
			get { return _attributeName; }
		}
		private readonly string _objectName;
		/// <summary>
		/// ObjectName of MBean
		/// </summary>
		public string ObjectName
		{
			get { return _objectName; }
		}
		private readonly string _className;
		/// <summary>
		/// Class name of MBean
		/// </summary>
		public string ClassName
		{
			get { return _className; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeName">Name of missing attribute</param>
		/// <param name="objectName">ObjectName of MBean</param>
		/// <param name="className">Class name of MBean</param>
		public AttributeNotFoundException(string attributeName, string objectName, string className)
			:
			 base(string.Format(CultureInfo.CurrentCulture, "Attribute \"{0}\" not found in MBean \"{1}\" of class \"{2}\"", attributeName, objectName, className))
		{
			_attributeName = attributeName;
			_objectName = objectName;
			_className = className;
		}

		private AttributeNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_attributeName = info.GetString("attributeName");
			_className = info.GetString("className");
			_objectName = info.GetString("objectName");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("attributeName", _attributeName);
			info.AddValue("className", _className);
			info.AddValue("objectName", _objectName);
		}
	}
}
