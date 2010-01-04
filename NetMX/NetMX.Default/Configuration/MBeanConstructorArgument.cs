#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
#endregion

namespace NetMX.Server.Configuration
{
   /// <summary>
   /// Represents an argument for MBean class constructor. Must have name and value specified, but currently
   /// only value is used to instanciate an MBean. Arguments must be placed in arguments collection in order
   /// in which they should be passed to constructor.
   /// </summary>
   public class MBeanConstructorArgument : ConfigurationElement
   {
      /// <summary>
      /// The name of the argument.
      /// </summary>
      [ConfigurationProperty("name", IsRequired=true, IsKey=true)]
      public string Name
      {
         get { return (string)this["name"]; }
         set { this["name"] = value; }
      }
      /// <summary>
      /// The value of the argument.
      /// </summary>
      [ConfigurationProperty("value", IsRequired = true)]
      public string Value
      {
         get { return (string)this["value"]; }
         set { this["value"] = value; }
      }
      /// <summary>
      /// The assembly qualified name of type of the argument.
      /// </summary>
      [ConfigurationProperty("type", IsRequired = true)]
      public string Type
      {
         get { return (string)this["type"]; }
         set { this["type"] = value; }
      }
   }
}