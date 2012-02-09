#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX.Monitor
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class NotObservedObjectException : OperationsException
	{
	   private readonly string _objectName;
		/// <summary>
		/// Gets the name of object which is reported not being observed.
		/// </summary>
		public string ObjectName
		{
			get { return _objectName; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
      /// <param name="objectName">The name of object which is reported not being observed..</param>
      public NotObservedObjectException(string objectName)
			: base(string.Format(CultureInfo.CurrentCulture, "Object \"{0}\" is not observed by this monitor.", objectName))
		{
         _objectName = objectName;
		}

      private NotObservedObjectException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
         _objectName = info.GetString("objectName");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
         info.AddValue("objectName", _objectName);
		}
	}
}
