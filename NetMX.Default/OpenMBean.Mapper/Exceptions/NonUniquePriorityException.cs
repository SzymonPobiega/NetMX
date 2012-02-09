using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NetMX.Server.OpenMBean.Mapper.Exceptions
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Other constructos do not make sense"), Serializable]
   public sealed class NonUniquePriorityException : NetMXException
   {
      private readonly string _failedMapper;
      private readonly string _existingMapper;
      private readonly int _priority;

      /// <summary>
      /// Creates new <see cref="NonUniquePriorityException"/> object.
      /// </summary>		
      public NonUniquePriorityException(string failedMapper, string existingMapper, int priority)
         : base(string.Format(CultureInfo.CurrentCulture, "Cannot register mapper {0} with priority {1} because another mapper ({2}) already uses this priority.", failedMapper, priority, existingMapper))
      {
         _failedMapper = failedMapper;
         _existingMapper = existingMapper;
         _priority = priority;
      }

      private NonUniquePriorityException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         _failedMapper = info.GetString("newMapper");
         _existingMapper = info.GetString("existingMapper");
         _priority = info.GetInt32("priority");
      }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("newMapper", _failedMapper);
         info.AddValue("existingMapper", _existingMapper);
         info.AddValue("priority", _priority);
      }

      /// <summary>
      /// Gets the CLR type or <see cref="ObjectName"/> of mapper which failed to be registered.
      /// </summary>
      public string FailedMapper
      {
         get { return _failedMapper; }
      }
      /// <summary>
      /// Gets the CLR type or <see cref="ObjectName"/> of mapper which caused the priority conflict.
      /// </summary>
      public string ExistingMapper
      {
         get { return _existingMapper; }
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