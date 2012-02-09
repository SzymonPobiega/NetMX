using System;
using System.Linq;

namespace NetMX.OpenMBean
{   
   /// <summary>
   /// Extension methods for <see cref="ITabularData"/> implementations.
   /// </summary>
   public static class TabularDataExtensions
   {
      /// <summary>
      /// Puts new row into tabular data instance.
      /// </summary>
      /// <param name="data">Tabular data instance.</param>
      /// <param name="rowBuilderAction">Action used to build new row.</param>
      /// <returns>This tabular data instance.</returns>
      public static ITabularData Put(this ITabularData data, Action<ICompositeDataBuilder> rowBuilderAction)
      {
         if (data == null)
         {
            throw new ArgumentNullException("data");
         }
         if (rowBuilderAction == null)
         {
            throw new ArgumentNullException("rowBuilderAction");
         }
         CompositeDataBuilder rowBuilder = new CompositeDataBuilder(data.TabularType.RowType);
         rowBuilderAction(rowBuilder);
         data.Put(rowBuilder.Create());
         return data;
      }
   }   
}