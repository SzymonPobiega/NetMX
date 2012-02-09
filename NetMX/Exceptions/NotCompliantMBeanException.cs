#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class NotCompliantMBeanException : OperationsException
	{
		private string _className;
		/// <summary>
		/// Class name of not compliant MBean.
		/// </summary>
		public string ClassName
		{
			get { return _className; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="objectName">ObjectName of not compliant MBean.</param>
		public NotCompliantMBeanException(string className)
			: base(string.Format(CultureInfo.CurrentCulture, "MBean class \"{0}\" is not compliant with NetMX specification.", className))
		{
			_className = className;
		}
		
		private NotCompliantMBeanException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_className = info.GetString("className");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("className", _className);
		}
	}
}
