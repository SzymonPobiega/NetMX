using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Builds <see cref="MBeanParameterInfo"/> object.
   /// </summary>
   public interface IParameterBuilder
   {
      /// <summary>
      /// Finishes build process by defining parameter's type.
      /// </summary>
      /// <param name="prameterType">Parameter type.</param>
      /// <returns>A factory method for creating just defined MBean operation parameter metadata.</returns>
      Func<MBeanParameterInfo> TypedAs(Type prameterType);
   }

   public static partial class MBean 
   {
      /// <summary>
      /// Defines <see cref="MBeanParameterInfo"/>.
      /// </summary>
      /// <param name="name">Name of the parameter.</param>
      /// <param name="description">Description of parameter.</param>
      /// <returns>Object which can be used to further define MBean operation parameter metadata.</returns>
      public static IParameterBuilder Parameter(string name, string description)
      {         
         return new ParameterBuilder(name, description);
      }

      private class ParameterBuilder : IParameterBuilder, IBuilder
      {
         private readonly string _name;
         private readonly string _description;
         private readonly Descriptor _descriptor = new Descriptor();

         public ParameterBuilder(string name, string description)
         {
            _name = name;
            _description = description;
         }

         public Func<MBeanParameterInfo> TypedAs(Type prameterType)
         {
            return () => new MBeanParameterInfo(_name, _description, prameterType.AssemblyQualifiedName, _descriptor);
         }

         public Descriptor Descriptor
         {
            get { return _descriptor; }
         }
      }
   }
}