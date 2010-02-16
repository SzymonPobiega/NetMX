using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Contains methods for fluent building of <see cref="TabularType"/>.
   /// </summary>
   public static class Tabular
   {
      /// <summary>
      /// Defines composite type.
      /// </summary>
      /// <param name="name">Name of tabular type.</param>
      /// <param name="description">Description of tabular type.</param>
      /// <returns>Object which can be used to further define this tabular type.</returns>
      public static TabularTypeBuilder Type(string name, string description)
      {
         return new TabularTypeBuilderImpl(name, description);
      }

      private class TabularTypeBuilderImpl : TabularTypeBuilder, ITabularTypeColumnBuilder
      {
         private readonly string _name;
         private readonly string _description;

         private readonly List<string> _names = new List<string>();
         private readonly List<string> _descriptions = new List<string>();
         private readonly List<OpenType> _types = new List<OpenType>();
         private readonly List<string> _index = new List<string>();

         public TabularTypeBuilderImpl(string name, string description)
         {
            _name = name;
            _description = description;
         }

         public override ITabularTypeColumnBuilder WithColumn(string name, string description)
         {
            _names.Add(name);
            _descriptions.Add(description);
            return this;
         }

         public override ITabularTypeColumnBuilder WithIndexColumn(string name, string description)
         {
            _index.Add(name);
            return WithColumn(name, description);
         }

         public override TabularType Build()
         {
            return new TabularType(_name, _description, new CompositeType(_name + " row", null, _names, _descriptions, _types), _index);
         }

         public TabularTypeBuilder TypedAs(OpenType openType)
         {
            _types.Add(openType);
            return this;
         }
      }
   }

   /// <summary>
   /// Builds <see cref="TabularType"/> object.
   /// </summary>
   public abstract class TabularTypeBuilder
   {
      /// <summary>
      /// Defines new tabular type's column with provided name and description. This column will be part of tabular type's index.
      /// </summary>
      /// <param name="name">Name of the column.</param>
      /// <param name="description">Description of the column.</param>
      /// <returns>Object which can be used to further define the column.</returns>
      public abstract ITabularTypeColumnBuilder WithIndexColumn(string name, string description);

      /// <summary>
      /// Defines new tabular type's column with provided name and description.
      /// </summary>
      /// <param name="name">Name of the column.</param>
      /// <param name="description">Description of the column.</param>
      /// <returns>Object which can be used to further define the column.</returns>
      public abstract ITabularTypeColumnBuilder WithColumn(string name, string description);

      /// <summary>
      /// Builds concrete composite type's instance.
      /// </summary>
      /// <returns>Instance of <see cref="CompositeType"/>.</returns>
      public abstract TabularType Build();

      /// <summary>
      /// Implicitly converts tabular type builder into <see cref="TabularType"/> instance.
      /// </summary>
      /// <param name="builder">Builder.</param>
      /// <returns>Built type instance.</returns>
      public static implicit operator TabularType(TabularTypeBuilder builder)
      {
         return builder.Build();
      }
   }

   /// <summary>
   /// Defines a column.
   /// </summary>
   public interface ITabularTypeColumnBuilder
   {
      /// <summary>
      /// Defines type of tabular type's column.
      /// </summary>
      /// <param name="openType">Type of this column.</param>
      /// <returns>Object which can be used to add subsequent columns.</returns>
      TabularTypeBuilder TypedAs(OpenType openType);
   }
}