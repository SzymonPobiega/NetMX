#region USING
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
#endregion

namespace NetMX.OpenMBean
{
   internal static class OpenInfoUtils
   {
      internal static ReadOnlyCollection<TDest> Transform<TDest, TSource>(IEnumerable<TSource> source)
      {
         List<TDest> results = new List<TDest>();
         foreach (TSource element in source)
         {
            results.Add((TDest)(object)element);
         }
         return results.AsReadOnly();
      }
   }
}
