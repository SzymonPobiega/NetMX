using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMX;

namespace NetMX.Spring.BeanExporter.Naming
{
   public sealed class KeyNamingStrategy : IObjectNamingStrategy
   {
      private readonly string _defaultDomainName;

      public KeyNamingStrategy(string defaultDomainName)
      {
         if (defaultDomainName == null)
         {
            throw new ArgumentNullException("defaultDomainName");
         }
         _defaultDomainName = defaultDomainName;
      }

      #region IObjectNamingStrategy Members
      public ObjectName GetObjectName(object managedBean, string beanKey)
      {
         Dictionary<string, string> properties = new Dictionary<string, string>();
         properties["name"] = beanKey;
         return new ObjectName(_defaultDomainName, properties);
      }
      #endregion
   }
}
