#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
#endregion

namespace NetMX
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class MBeanRegistrationException : MBeanException
	{
		private string _phase;
		/// <summary>
		/// Registration phase.
		/// </summary>
		public string Phase
		{
			get { return _phase; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="phase">Registration phase.</param>
		/// <param name="inner">Thrown exception.</param>
		public MBeanRegistrationException(string phase, Exception inner)
			: base(string.Format(CultureInfo.CurrentCulture, "Exception thrown in {0} phase", phase), inner)
		{
			_phase = phase;
		}
		private MBeanRegistrationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_phase = info.GetString("phase");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("phase", _phase);
		}
	}
}
