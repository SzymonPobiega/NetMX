using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NetMX.OpenMBean.Mapper
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Other constructos do not make sense"), Serializable]
   public sealed class MapperNotFoundException : NetMXException
   {
      private readonly int _priority;

      /// <summary>
      /// Creates new <see cref="MapperNotFoundException"/> object.
		/// </summary>		
		public MapperNotFoundException(int priority)
			: base(string.Format(CultureInfo.CurrentCulture, "Cannot find mapper with priority {0}.",priority))
		{         
         _priority = priority;
		}

      private MapperNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_priority = info.GetInt32("priority");
		}
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
			info.AddValue("priority", _priority);
      }
		
		/// <summary>
		/// Gets the priority.
		/// </summary>
		public int Priority
		{
			get { return _priority; }
		}
   }
}
