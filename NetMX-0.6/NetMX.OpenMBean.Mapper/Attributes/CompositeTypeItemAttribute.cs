using System;

namespace NetMX.OpenMBean.Mapper
{
   /// <summary>
   /// Provides information about name mapping for a property.
   /// </summary>
   [AttributeUsage(AttributeTargets.Property)]
   public sealed class CompositeTypeItemAttribute : Attribute
   {
      private readonly string _mappedName;

      /// <summary>
      /// Creates new <see cref="CompositeTypeItemAttribute"/> instance.
      /// </summary>
      /// <param name="mappedName">Name of <see cref="CompositeType"/> item the property is to be mapped to.</param>
      public CompositeTypeItemAttribute(string mappedName)
      {
         _mappedName = mappedName;
      }

      /// <summary>
      /// Gets the name of <see cref="CompositeType"/> item the property is to be mapped to.
      /// </summary>
      public string MappedName
      {
         get { return _mappedName; }
      }      
   }
}
