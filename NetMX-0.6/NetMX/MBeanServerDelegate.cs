#region USING
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace NetMX
{
   /// <summary>
   /// Represents the MBean server from the management point of view. The MBeanServerDelegate MBean emits the 
   /// MBeanServerNotifications when an MBean is registered/unregistered in the MBean server.
   /// </summary>
   public class MBeanServerDelegate : NotificationEmitterSupport, MBeanServerDelegateMBean, IMBeanRegistration
   {
      #region Const
      public const string ObjectName = "NetMXImplementation:type=MBeanServerDelegate";
      #endregion

      #region MEMBERS
      private string _serverId;
      private string _implementationName;
      private string _implementationVendor;
      private string _implementationVersion;
      #endregion

      #region PROPERTIES
      #endregion

      #region CONSTRUCTOR
      /// <summary>      
      /// Creates new MBeanServerDelegate object.      
      /// </summary>
      /// <param name="serverId">The MBean server agent identity.</param>
      /// <param name="implementationName">The NetMX implementation name (the name of this product).</param>
      /// <param name="implementationVendor">The NetMX implementation vendor (the vendor of this product).</param>
      /// <param name="implementationVersion">The NetMX implementation version (the version of this product).</param>
      public MBeanServerDelegate(string serverId, string implementationName, string implementationVendor, string implementationVersion)
      {
         _serverId = serverId;
         _implementationName = implementationName;
         _implementationVendor = implementationVendor;
         _implementationVersion = implementationVersion;
      }
      #endregion

      #region MBeanServerDelegateMBean Members
      public string ImplementationName
      {
         get { return _implementationName; }
      }

      public string ImplementationVendor
      {
         get { return _implementationVendor; }
      }

      public string ImplementationVersion
      {
         get { return _implementationVersion; }
      }

      public string MBeanServerId
      {
         get { return _serverId; }
      }

      public string SpecificationName
      {
         get { return "NetMX"; }
      }

      public string SpecificationVendor
      {
         get { return "http://codeplex.com/NetMX"; }
      }

      public string SpecificationVersion
      {
         get { return "1.0"; }
      }
      #endregion

      #region IMBeanRegistration Members
      public void PostDeregister()
      {
      }
      public void PostRegister(bool registrationDone)
      {
      }
      public void PreDeregister()
      {
      }
      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         base.Initialize(name, new MBeanNotificationInfo[] {
            new MBeanNotificationInfo(new string[] { MBeanServerNotification.RegistrationNotification,
               MBeanServerNotification.UnregistrationNotification }, typeof(MBeanServerNotification).AssemblyQualifiedName, null)});
         return name;
      }
      #endregion
   }
}
