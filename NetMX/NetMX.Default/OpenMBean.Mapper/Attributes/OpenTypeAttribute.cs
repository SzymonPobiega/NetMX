using System;
using NetMX.OpenMBean;

namespace NetMX.Server.OpenMBean.Mapper.Attributes
{
   /// <summary>
   /// Provides information about mapping of CLR type to <see cref="OpenType"/>.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false)]
   public sealed class OpenTypeAttribute : Attribute
   {
      private string _mappedName;
      private string _resourceName;

      /// <summary>
      /// Name of <see cref="OpenType"/> this type is mapped to. Default is full name of CLR type.
      /// </summary>
      public string MappedName
      {
         get { return _mappedName; }
         set { _mappedName = value; }
      }      
      /// <summary>
      /// Name of resource file containing textual description of mapped <see cref="OpenType"/> and its
      /// features.
      /// </summary>
      public string ResourceName
      {
         get { return _resourceName; }
         set { _resourceName = value; }
      }
   }
}