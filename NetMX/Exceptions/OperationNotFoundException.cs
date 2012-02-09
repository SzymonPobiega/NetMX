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
	public sealed class OperationNotFoundException : OperationsException
	{
		private string _operationName;
		/// <summary>
		/// Missing operation name.
		/// </summary>
		public string OperationName
		{
			get { return _operationName; }
		}
		private string _objectName;
		/// <summary>
		/// ObjectName of MBean
		/// </summary>
		public string ObjectName
		{
			get { return _objectName; }
		}
		private string _className;
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
		/// <param name="operationName">Name of missing operation.</param>
		/// <param name="objectName">ObjectName of MBean.</param>
		/// <param name="className">Class name of MBean.</param>
		public OperationNotFoundException(string operationName, string objectName, string className)
			:
			 base(string.Format(CultureInfo.CurrentCulture, "Operation \"{0}\" not found in MBean \"{1}\" of class \"{2}\"", operationName, objectName, className))
		{
			_operationName = operationName;
			_objectName = objectName;
			_className = className;
		}
		
		private OperationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			_operationName = info.GetString("operationName");
			_className = info.GetString("className");
			_objectName = info.GetString("objectName");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("operationName", _operationName);
			info.AddValue("className", _className);
			info.AddValue("objectName", _objectName);
		}
	}
}
