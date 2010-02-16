using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Contains methods for fluent building of <see cref="CompositeType"/>.
   /// </summary>
   public static class Composite
   {
      /// <summary>
      /// Defines composite type.
      /// </summary>
      /// <param name="name">Name of composite type.</param>
      /// <param name="description">Description of composite type.</param>
      /// <returns>Object which can be used to further define this composite type.</returns>
      public static CompositeTypeBuilder Type(string name, string description)
      {
         return new CompositeTypeBuilderImpl(name, description);
      }

      private class CompositeTypeBuilderImpl : CompositeTypeBuilder, ICompositeTypeItemBuilder
      {
         private readonly string _name;
         private readonly string _description;

         private readonly List<string> _names = new List<string>();
         private readonly List<string> _descriptions = new List<string>();
         private readonly List<OpenType> _types = new List<OpenType>();

         public CompositeTypeBuilderImpl(string name, string description)
         {
            _name = name;
            _description = description;
         }

         public override ICompositeTypeItemBuilder WithItem(string name, string description)
         {
            _names.Add(name);
            _descriptions.Add(description);
            return this;
         }

         public override CompositeType Build()
         {
            return new CompositeType(_name, _description, _names, _descriptions, _types);
         }

         public CompositeTypeBuilder TypedAs(OpenType openType)
         {
            _types.Add(openType);
            return this;
         }
      }
   }

   /// <summary>
   /// Builds <see cref="CompositeType"/> object.
   /// </summary>
   public abstract class CompositeTypeBuilder
   {
      /// <summary>
      /// Defines new composite type's item with provided name and description.
      /// </summary>
      /// <param name="name">Item's name.</param>
      /// <param name="description">Item's description.</param>
      /// <returns>Object which can be used to further define composite type's item.</returns>
      public abstract ICompositeTypeItemBuilder WithItem(string name, string description);

      /// <summary>
      /// Builds concrete composite type's instance.
      /// </summary>
      /// <returns>Instance of <see cref="CompositeType"/>.</returns>
      public abstract CompositeType Build();

      /// <summary>
      /// Implicitly converts composite type builder into <see cref="CompositeType"/> instance.
      /// </summary>
      /// <param name="builder">Builder.</param>
      /// <returns>Built type instance.</returns>
      public static implicit operator CompositeType(CompositeTypeBuilder builder)
      {
         return builder.Build();
      }
   }

   /// <summary>
   /// Defines composite type item.
   /// </summary>
   public interface ICompositeTypeItemBuilder
   {
      /// <summary>
      /// Defines type of composite type's item.
      /// </summary>
      /// <param name="openType">Type of this item.</param>
      /// <returns>Object which can be used to add subsequent items.</returns>
      CompositeTypeBuilder TypedAs(OpenType openType);
   }
}