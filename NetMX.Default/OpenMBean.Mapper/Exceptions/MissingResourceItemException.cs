using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NetMX.Server.OpenMBean.Mapper.Exceptions
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "Other constructos do not make sense"), Serializable]
   public sealed class MissingResourceItemException : NetMXException
   {
      private readonly string _itemName;
      private readonly string _resourceName;
      private readonly string _assemblyName;

      /// <summary>
      /// Creates new <see cref="MissingResourceItemException"/> object.
      /// </summary>		
      public MissingResourceItemException(string itemName, string resourceName, string assemblyName)
         : base(string.Format(CultureInfo.CurrentCulture, "Unable to retrieve item \"{0}\" from resource \"{1}\" of assembly {2}.", itemName, resourceName, assemblyName))
      {
         _itemName = itemName;
         _resourceName = resourceName;
         _assemblyName = assemblyName;
      }

      private MissingResourceItemException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
         _itemName = info.GetString("itemName");
         _resourceName = info.GetString("resourceName");
         _assemblyName = info.GetString("assemblyName");
      }
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue("itemName", _itemName);
         info.AddValue("resourceName", _resourceName);
         info.AddValue("assemblyName", _assemblyName);
      }

      /// <summary>
      /// Name of resource item that failed to be retrieved.
      /// </summary>
      public string ItemName
      {
         get { return _itemName; }
      }
      /// <summary>
      /// Resource file name that was to be searched.
      /// </summary>
      public string ResourceName
      {
         get { return _resourceName; }
      }
      /// <summary>
      /// Assembly containing resource being searched for.
      /// </summary>
      public string AssemblyName
      {
         get { return _assemblyName; }
      }      
   }
}