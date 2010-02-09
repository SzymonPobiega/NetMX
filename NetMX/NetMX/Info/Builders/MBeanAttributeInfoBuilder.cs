using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMX
{
   /// <summary>
   /// Builds <see cref="MBeanAttributeInfo"/> object.
   /// </summary>
   public interface IAttributeBuilder
   {
      /// <summary>
      /// Finishes build process by defining attribute's type.
      /// </summary>
      /// <param name="attributeType">Attribute type.</param>
      /// <returns>A factory method for creating just defined MBean attribute metadata.</returns>
      Func<MBeanAttributeInfo> TypedAs(Type attributeType);
   }

   public static partial class MBean
   {
      /// <summary>
      /// Defines <see cref="MBeanAttributeInfo"/>.
      /// </summary>
      /// <param name="name">Name of the parameter.</param>
      /// <param name="description">Description of parameter.</param>
      /// <param name="readable">Is this attribute readable?</param>
      /// <param name="writable">Is this attribute writable?</param>
      /// <returns>Object which can be used to further define MBean attribute metadata.</returns>
      public static IAttributeBuilder Attribute(string name, string description, bool readable, bool writable)
      {
         return new AttributeBuilder(name, description, readable, writable);
      }

      /// <summary>
      /// Defines <see cref="MBeanAttributeInfo"/> for a read-only attribute.
      /// </summary>
      /// <param name="name">Name of the parameter.</param>
      /// <param name="description">Description of parameter.</param>
      /// <returns>Object which can be used to further define MBean attribute metadata.</returns>
      public static IAttributeBuilder ReadOnlyAttribute(string name, string description)
      {
         return new AttributeBuilder(name, description, true, false);
      }

      /// <summary>
      /// Defines <see cref="MBeanAttributeInfo"/> for a read-and-write attribute.
      /// </summary>
      /// <param name="name">Name of the parameter.</param>
      /// <param name="description">Description of parameter.</param>
      /// <returns>Object which can be used to further define MBean attribute metadata.</returns>
      public static IAttributeBuilder WritableAttribute(string name, string description)
      {
         return new AttributeBuilder(name, description, true, true);
      }

      private class AttributeBuilder : IAttributeBuilder, IBuilder
      {
         private readonly string _name;
         private readonly string _description;
         private readonly bool _readable;
         private readonly bool _writable;
         private readonly Descriptor _descriptor = new Descriptor();

         public AttributeBuilder(string name, string description, bool readable, bool writable)
         {
            _name = name;
            _description = description;
            _readable = readable;
            _writable = writable;
         }

         public Func<MBeanAttributeInfo> TypedAs(Type attributeType)
         {
            return () => new MBeanAttributeInfo(_name, _description, attributeType.AssemblyQualifiedName, _readable, _writable, _descriptor);
         }

         public Descriptor Descriptor
         {
            get { return _descriptor; }
         }
      }
   }
}
