#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
#endregion

namespace NetMX.Configuration
{
   public static class TypedConfigurationManager
   {
      public static T GetSection<T>(string sectionName)
         where T : ConfigurationSection
      {
         return GetSection<T>(sectionName, true);
      }
      public static T GetSection<T>(string sectionName, bool throwIfNotFound )
         where T : ConfigurationSection
      {
         object section = System.Configuration.ConfigurationManager.GetSection(sectionName);
         if (section == null && throwIfNotFound)
         {
            throw new ArgumentException("Section not found");
         }
         return (T)section;
      }
   }
}