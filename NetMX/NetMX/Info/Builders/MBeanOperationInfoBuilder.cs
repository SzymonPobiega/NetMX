using System;
using System.Collections.Generic;
using System.Text;

namespace NetMX
{
   /// <summary>
   /// Builds <see cref="MBeanOperationInfo"/> object.
   /// </summary>
   public interface IOperationBuilder
   {
      /// <summary>
      /// Defines notifications of MBeanInfo being built and finished the building process.
      /// </summary>
      /// <param name="parameters">Factory method for obtaining collection of MBean operation parameter metadata.</param>
      /// <returns>Object which can be used to further define MBean operation metadata.</returns>
      IReturnTypeBuilder WithParameters(Func<IEnumerable<MBeanParameterInfo>> parameters);
   }

   /// <summary>
   /// Builds <see cref="MBeanOperationInfo"/> object.
   /// </summary>
   public interface IReturnTypeBuilder
   {
      /// <summary>
      /// Finishes the building process by defining operation's return type.
      /// </summary>
      /// <param name="type">Return type of this operation.</param>
      /// <returns>A factory method for creating just defined MBean operation metadata.</returns>
      Func<MBeanOperationInfo> Returning(Type type);
   }
   
   public static partial class MBean
   {
      /// <summary>
      /// Defines <see cref="MBeanOperationInfo"/> for operation with unknown impact.
      /// Cannot be uses in Open MBeans.
      /// </summary>
      /// <param name="name">Name of the operation.</param>
      /// <param name="description">Description of the operation.</param>
      /// <returns>Object which can be used to further define MBean operation metadata.</returns>
      public static IOperationBuilder Operation(string name, string description)
      {
         return new OperationBuilder(name, description, OperationImpact.Unknown);
      }

      /// <summary>
      /// Defines <see cref="MBeanOperationInfo"/> for mutator operation (one with side effects).
      /// </summary>
      /// <param name="name">Name of the operation.</param>
      /// <param name="description">Description of the operation.</param>
      /// <returns>Object which can be used to further define MBean operation metadata.</returns>
      public static IOperationBuilder MutatorOperation(string name, string description)
      {
         return new OperationBuilder(name, description, OperationImpact.Action);
      }

      /// <summary>
      /// Defines <see cref="MBeanOperationInfo"/> for query operation (one without side effects).
      /// </summary>
      /// <param name="name">Name of the operation.</param>
      /// <param name="description">Description of the operation.</param>
      /// <returns>Object which can be used to further define MBean operation metadata.</returns>
      public static IOperationBuilder QueryOperation(string name, string description)
      {
         return new OperationBuilder(name, description, OperationImpact.Info);
      }

      private class OperationBuilder : IOperationBuilder, IReturnTypeBuilder, IBuilder
      {
         private readonly string _name;
         private readonly string _description;
         private readonly OperationImpact _impact;
         private Func<IEnumerable<MBeanParameterInfo>> _parameters;
         private readonly Descriptor _descriptor = new Descriptor();

         public OperationBuilder(string name, string description, OperationImpact impact)
         {
            _name = name;
            _impact = impact;
            _description = description;
         }

         public IReturnTypeBuilder WithParameters(Func<IEnumerable<MBeanParameterInfo>> parameters)
         {
            _parameters = parameters;
            return this;
         }

         public Func<MBeanOperationInfo> Returning(Type type)
         {
            return () => new MBeanOperationInfo(_name, _description, type.AssemblyQualifiedName, _parameters(), _impact, _descriptor);
         }

         public Descriptor Descriptor
         {
            get { return _descriptor; }
         }
      }
   }   
}
