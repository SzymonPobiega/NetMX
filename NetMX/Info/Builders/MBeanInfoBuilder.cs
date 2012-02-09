using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetMX
{
   /// <summary>
   /// Builds <see cref="MBeanInfo"/> object.
   /// </summary>
   public interface IMBeanInfoBuilderAttributes : IMBeanInfoBuilderOperations
   {
      /// <summary>
      /// Defines attributes of MBeanInfo being built.
      /// </summary>
      /// <param name="attributes">Factory method for obtaining collection of MBean attribute metadata.</param>
      /// <returns>Object which can be used to further define MBean metadata.</returns>
      IMBeanInfoBuilderOperations WithAttributes(Func<IEnumerable<MBeanAttributeInfo>> attributes);
   }

   /// <summary>
   /// Builds <see cref="MBeanInfo"/> object.
   /// </summary>
   public interface IMBeanInfoBuilderOperations : IMBeanInfoBuilderConstructors
   {
      /// <summary>
      /// Defines operations of MBeanInfo being built.
      /// </summary>
      /// <param name="operations">Factory method for obtaining collection of MBean operation metadata.</param>
      /// <returns>Object which can be used to further define MBean metadata.</returns>
      IMBeanInfoBuilderConstructors WithOperations(Func<IEnumerable<MBeanOperationInfo>> operations);      
   }

   /// <summary>
   /// Builds <see cref="MBeanInfo"/> object.
   /// </summary>
   public interface IMBeanInfoBuilderConstructors : IMBeanInfoBuilderNotifications
   {
      /// <summary>
      /// Defines constructors of MBeanInfo being built.
      /// </summary>
      /// <param name="constructors">Factory method for obtaining collection of MBean constructor metadata.</param>
      /// <returns>Object which can be used to further define MBean metadata.</returns>
      IMBeanInfoBuilderNotifications WithConstructors(Func<IEnumerable<MBeanConstructorInfo>> constructors);
   }

   /// <summary>
   /// Builds <see cref="MBeanInfo"/> object.
   /// </summary>
   public interface IMBeanInfoBuilderNotifications
   {
      /// <summary>
      /// Defines notifications of MBeanInfo being built and finished the building process.
      /// </summary>
      /// <param name="notifications">Factory method for obtaining collection of MBean constructor metadata.</param>
      /// <returns>A factory method for creating just defined MBean metadata.</returns>
      Func<MBeanInfo> WithNotifications(Func<IEnumerable<MBeanNotificationInfo>> notifications);
      /// <summary>
      /// Finishes the building process.
      /// </summary>
      /// <returns>A factory method for creating just defined MBean metadata.</returns>
      Func<MBeanInfo> AndNothingElse();
   }

   /// <summary>
   /// Allows convenient MBean metadata building.
   /// </summary>
   public static partial class MBean
   {      
      /// <summary>
      /// Defines <see cref="MBeanInfo"/> with provided class name and description.
      /// </summary>
      /// <param name="className">Name of MBean class.</param>
      /// <param name="description">Description of this MBean.</param>
      /// <returns>Object which can be used to further define MBean metadata.</returns>
      public static IMBeanInfoBuilderAttributes Info(string className, string description)
      {
         return new MBeanInfoBuilder(className, description);
      }

      /// <summary>
      /// Defines <see cref="MBeanInfo"/> for provied class and with provided description.
      /// </summary>
      /// <typeparam name="T">Class of this MBean.</typeparam>
      /// <param name="description">Description of this MBean.</param>
      /// <returns>Object which can be used to further define MBean metadata.</returns>
      public static IMBeanInfoBuilderAttributes Info<T>(string description)
      {
         return new MBeanInfoBuilder(typeof(T).AssemblyQualifiedName, description);
      }

      private class MBeanInfoBuilder : IMBeanInfoBuilderAttributes
      {
         private static readonly MBeanAttributeInfo[] _emptyAttributes = new MBeanAttributeInfo[] {};
         private static readonly MBeanConstructorInfo[] _emptyConstructors = new MBeanConstructorInfo[] {};
         private static readonly MBeanOperationInfo[] _emptyOperations = new MBeanOperationInfo[] {};
         private static readonly MBeanNotificationInfo[] _emptyNotifications = new MBeanNotificationInfo[] {};

         private static readonly Func<IEnumerable<MBeanAttributeInfo>> _emptyAttributesGetter = () => _emptyAttributes;
         private static readonly Func<IEnumerable<MBeanOperationInfo>> _emptyOperationsGetter = () => _emptyOperations;
         private static readonly Func<IEnumerable<MBeanConstructorInfo>> _emptyConstructorsGetter = () => _emptyConstructors;

         private readonly string _className;
         private readonly string _description;
         private Func<IEnumerable<MBeanAttributeInfo>> _attributes = _emptyAttributesGetter;
         private Func<IEnumerable<MBeanConstructorInfo>> _constructors = _emptyConstructorsGetter;
         private Func<IEnumerable<MBeanOperationInfo>> _operations = _emptyOperationsGetter;

         public MBeanInfoBuilder(string className, string description)
         {
            _className = className;
            _description = description;
         }

         public IMBeanInfoBuilderOperations WithAttributes(Func<IEnumerable<MBeanAttributeInfo>> attributes)
         {
            _attributes = attributes;
            return this;
         }

         public IMBeanInfoBuilderNotifications WithConstructors(Func<IEnumerable<MBeanConstructorInfo>> constructors)
         {
            _constructors = constructors;
            return this;
         }

         public Func<MBeanInfo> WithNotifications(Func<IEnumerable<MBeanNotificationInfo>> notifications)
         {
            return () => new MBeanInfo(_className, _description, _attributes(), _constructors(), _operations(), notifications());
         }

         public Func<MBeanInfo> AndNothingElse()
         {
            return () => new MBeanInfo(_className, _description, _attributes(), _constructors(), _operations(), _emptyNotifications);
         }

         public IMBeanInfoBuilderConstructors WithOperations(Func<IEnumerable<MBeanOperationInfo>> operations)
         {
            _operations = operations;
            return this;
         }
      }
   }

   
}
