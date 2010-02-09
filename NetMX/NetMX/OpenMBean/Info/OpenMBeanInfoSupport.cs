#region Using
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// The OpenMBeanInfoSupport class describes the management information of an open MBean: it wrapps an instance 
   /// of <see cref="MBeanInfo"/>, and it implements the <see cref="IOpenMBeanInfo"/> interface. Note that an 
   /// open MBean is recognized as such if its info's descriptor contains OpenType field.
   /// </summary>
   [Serializable]
   public class OpenMBeanInfoSupport : IOpenMBeanInfo
   {
      private readonly MBeanInfo _wrappedInfo;
      private readonly IList<IOpenMBeanAttributeInfo> _wrappedAttributes;
      private readonly IList<IOpenMBeanOperationInfo> _wrappedOperations;
      private readonly IList<IOpenMBeanConstructorInfo> _wrappedConstructors;

      /// <summary>
      /// Creates new open MBean info wrapper.
      /// </summary>
      /// <param name="wrappedInfo"></param>
      public OpenMBeanInfoSupport(MBeanInfo wrappedInfo)
      {
         _wrappedInfo = wrappedInfo;
         _wrappedAttributes = _wrappedInfo.Attributes.Select<MBeanAttributeInfo, IOpenMBeanAttributeInfo>(x => new OpenMBeanAttributeInfoSupport(x)).ToList().AsReadOnly();
         _wrappedOperations = _wrappedInfo.Operations.Select<MBeanOperationInfo, IOpenMBeanOperationInfo>(x => new OpenMBeanOperationInfoSupport(x)).ToList().AsReadOnly();
         _wrappedConstructors = _wrappedInfo.Constructors.Select<MBeanConstructorInfo, IOpenMBeanConstructorInfo>(x => new OpenMBeanConstructorInfoSupport(x)).ToList().AsReadOnly();
      }

      public string ClassName
      {
         get { return _wrappedInfo.ClassName; }
      }

      public string Description
      {
         get { return _wrappedInfo.Description; }
      }

      public IList<IOpenMBeanAttributeInfo> Attributes
      {
         get { return _wrappedAttributes; }
      }

      public IList<IOpenMBeanOperationInfo> Operations
      {
         get { return _wrappedOperations; }
      }

      public IList<IOpenMBeanConstructorInfo> Constructors
      {
         get { return _wrappedConstructors; }
      }

      public IList<MBeanNotificationInfo> Notifications
      {
         get { return _wrappedInfo.Notifications; }
      }
   }
}