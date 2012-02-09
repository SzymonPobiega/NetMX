using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.Remote.Jsr262
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> nullableCollection)
      {
         if (nullableCollection != null)
         {
            return nullableCollection;
         }
         return new T[] { };
      }
   }
}