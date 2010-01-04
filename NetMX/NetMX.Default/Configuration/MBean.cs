#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace NetMX.Server.Configuration
{
   /// <summary>
   /// Represents one MBean instance. Defines name, class and construcotr arguments for this bean. Values are
   /// to be passed to <see cref="IMBeanServer.CreateMBean"/> method.
   /// </summary>
   public class MBean : ConfigurationElement
   {
      /// <summary>
      /// Object name of configured MBean. Configuration framework ensures uniqueness of is among all MBean
      /// declarations, but user must ensure that no other non-configuration-created MBeans use this name.
      /// </summary>
      [ConfigurationProperty("objectName", IsKey=true, IsRequired=true)]
      public string ObjectName
      {
         get { return (string)this["objectName"]; }
         set { this["objectName"] = value; }
      }
      /// <summary>
      /// Assembly qualified name of this MBean's class.
      /// </summary>
      [ConfigurationProperty("className", IsRequired=true)]
      public string ClassName
      {
         get { return (string)this["className"]; }
         set { this["className"] = value; }
      }
      /// <summary>
      /// Collection of argument of this MBean's class constructor. Arguments in the collection must be placed in the
      /// same order as in the constructor signature.
      /// </summary>
      [ConfigurationProperty("arguments", IsRequired=true)]		
      public MBeanConstructorArgumentCollection Arguments
      {
         get { return (MBeanConstructorArgumentCollection)this["arguments"]; }
         set { this["arguments"] = value; }
      }
   }
}