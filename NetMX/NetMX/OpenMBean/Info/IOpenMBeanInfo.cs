#region Using
using System.Collections.Generic;
using NetMX;
#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes an Open MBean: an Open MBean is recognized as such if its <see cref="IDynamicMBean.GetMBeanInfo"/> method returns an 
   /// instance of a class which implements the OpenMBeanInfo interface, typically OpenMBeanInfoSupport.
   /// 
   /// This interface declares the same methods as the class <see cref="MBeanInfo"/>. A class implementing 
   /// this interface (typically OpenMBeanInfoSupport) should extend <see cref="MBeanInfo"/>.
   ///    
   /// The <see cref="Attributes"/>, <see cref="Operations"/> and <see cref="Constructors"/> properties of the 
   /// implementing class should return at runtime a list of instances of a subclass of <see cref="MBeanAttributeInfo"/>, 
   /// <see cref="MBeanOperationInfo"/> or <see cref="MBeanConstructorInfo"/> respectively which implement the 
   /// <see cref="IOpenMBeanAttributeInfo"/>, <see cref="IOpenMBeanOperationInfo"/> or 
   /// <see cref="IOpenMBeanConstructorInfo"/> interface respectively.
   /// </summary>
   public interface IOpenMBeanInfo
   {      
      /// <summary>
      /// Gets the assembly-qualified class name of the open MBean instances this IOpenMBeanInfo describes.
      /// </summary>
      string ClassName { get;}
      /// <summary>
      /// Gets a human readable description of the type of open MBean instances this IOpenMBeanInfo describes.
      /// </summary>
      string Description { get; }
      /// <summary>
      /// Gets a list of <see cref="IOpenMBeanAttributeInfo"/> instances describing each attribute in the open MBean 
      /// described by this IOpenMBeanInfo instance. Each instance in the returned list should actually be a 
      /// subclass of <see cref="MBeanAttributeInfo"/> which implements the <see cref="IOpenMBeanAttributeInfo"/> 
      /// interface (typically OpenMBeanAttributeInfoSupport).
      /// </summary>
      IList<IOpenMBeanAttributeInfo> Attributes { get; }
      /// <summary>
      /// Gets a list of <see cref="IOpenMBeanOperationInfo"/> instances describing each operation in the open 
      /// MBean described by this IOpenMBeanInfo instance. Each instance in the returned list should actually 
      /// be a subclass of <see cref="MBeanOperationInfo"/> which implements the <see cref="IOpenMBeanOperationInfo"/> 
      /// interface (typically OpenMBeanOperationInfoSupport).
      /// </summary>
      IList<IOpenMBeanOperationInfo> Operations { get; }
      /// <summary>
      /// Gets a list of <see cref="IOpenMBeanConstructorInfo"/> instances describing each constructor in 
      /// the open MBean described by this IOpenMBeanInfo instance. Each instance in the returned list should 
      /// actually be a subclass of <see cref="MBeanConstructorInfo"/> which implements the 
      /// <see cref="IOpenMBeanConstructorInfo"/> interface (typically OpenMBeanConstructorInfoSupport).
      /// </summary>
      IList<IOpenMBeanConstructorInfo> Constructors { get; }
      /// <summary>
      /// Gets a list of <see cref="MBeanNotificationInfo"/> instances describing each notification emitted by 
      /// the open MBean described by this IOpenMBeanInfo instance.
      /// </summary>
      IList<MBeanNotificationInfo> Notifications { get; }
   }
}