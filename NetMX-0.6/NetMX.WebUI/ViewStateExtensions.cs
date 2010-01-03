using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace NetMX.WebUI.WebControls
{   
   internal static class ViewStateExtensions
   {
      public static T TryGetValue<T>(StateBag bag, string key)
      {
         return TryGetValue(bag, key, default(T));
      }

      public static T TryGetValue<T>(StateBag bag, string key, T defaultValue)
      {
         if (bag[key] != null)
         {
            return (T) bag[key];
         }
         return defaultValue;
      }
   }
}
