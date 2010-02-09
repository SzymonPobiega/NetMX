using System;
using System.Collections.Generic;
using System.Text;
using NetMX.OpenMBean;

namespace NetMX.Server.OpenMBean.Mapper
{
   internal class ProxyBean : NotificationEmitterSupport, IDynamicMBean, IMBeanRegistration
   {
      private readonly MBeanInfo _info;
      private ObjectName _ownName;
      private readonly ObjectName _originalName;
      private IMBeanServer _server;
      private readonly Dictionary<string, OpenAndClrType> _attributeTypes = new Dictionary<string, OpenAndClrType>();
      private readonly Dictionary<string, OpenAndClrType> _operationReturnTypes = new Dictionary<string, OpenAndClrType>();
      private readonly OpenTypeCache _typeCache;

      public ProxyBean(MBeanInfo originalBeanInfo, ObjectName originalName, OpenTypeCache typeCache)
      {
         _originalName = originalName;
         _typeCache = typeCache;

         List<MBeanAttributeInfo> attributes = new List<MBeanAttributeInfo>();
         List<MBeanOperationInfo> operations = new List<MBeanOperationInfo>();
         List<MBeanConstructorInfo> constructors = new List<MBeanConstructorInfo>();

         foreach (MBeanAttributeInfo attributeInfo in originalBeanInfo.Attributes)
         {
            if (attributeInfo.Readable)
            {
               Type attributeType = Type.GetType(attributeInfo.Type, true);
               OpenType mappedType = _typeCache.MapType(attributeType);
               if (mappedType != null)
               {
                  Descriptor descriptor = new Descriptor(); //TODO: Copty fields
                  descriptor.SetField(OpenTypeDescriptor.Field, mappedType);
                  MBeanAttributeInfo openInfo = new MBeanAttributeInfo(
                     attributeInfo.Name, attributeInfo.Description, mappedType.Representation.AssemblyQualifiedName,
                     attributeInfo.Readable, false, descriptor);
                  attributes.Add(openInfo);
                  _attributeTypes[attributeInfo.Name] = new OpenAndClrType(attributeType, mappedType);
               }
            }
         }
         foreach (MBeanOperationInfo operationInfo in originalBeanInfo.Operations)
         {            
            Type returnType = Type.GetType(operationInfo.ReturnType, true);
            OpenType mappedReturnType = _typeCache.MapType(returnType);
            if (mappedReturnType == null)
            {
               continue;               
            }
            Descriptor descriptor = new Descriptor();
            descriptor.SetField(OpenTypeDescriptor.Field, mappedReturnType);
            bool success = true;
            List<MBeanParameterInfo> openParameters = new List<MBeanParameterInfo>();
            foreach (MBeanParameterInfo parameterInfo in operationInfo.Signature)
            {
               OpenType mappedParamType = _typeCache.MapType(Type.GetType(parameterInfo.Type, true));
               if (mappedParamType == null || mappedParamType.Kind != OpenTypeKind.SimpleType)
               {
                  success = false;
                  break;                  
               }
               Descriptor paramDescriptor = new Descriptor(); //TODO: Copy fields
               paramDescriptor.SetField(OpenTypeDescriptor.Field, mappedParamType);
               openParameters.Add(new MBeanParameterInfo(parameterInfo.Name, parameterInfo.Description,
                                                         mappedParamType.Representation.AssemblyQualifiedName,
                                                         paramDescriptor));
            }
            if (!success)
            {
               continue;               
            }
            MBeanOperationInfo openInfo = new MBeanOperationInfo(operationInfo.Name, operationInfo.Description,
                                                                 mappedReturnType.Representation.AssemblyQualifiedName,
                                                                 openParameters, operationInfo.Impact, descriptor);
            operations.Add(openInfo);
            _operationReturnTypes[operationInfo.Name] = new OpenAndClrType(returnType, mappedReturnType);
         }

         _info = new MBeanInfo(originalBeanInfo.ClassName, originalBeanInfo.Description,
                                          attributes, constructors, operations, originalBeanInfo.Notifications);
      }

      #region IDynamicMBean Members
      public MBeanInfo GetMBeanInfo()
      {
         return _info;
      }
      public object GetAttribute(string attributeName)
      {
         OpenAndClrType type;
         if (!_attributeTypes.TryGetValue(attributeName, out type))
         {
            throw new AttributeNotFoundException(attributeName, _ownName, _info.ClassName);
         }         
         object originalValue = _server.GetAttribute(_originalName, attributeName);
         return _typeCache.MapValue(type.ClrType, type.OpenType, originalValue);
      }
      public void SetAttribute(string attributeName, object value)
      {
         throw new NotImplementedException();
      }
      public object Invoke(string operationName, object[] arguments)
      {
         OpenAndClrType type;
         if (!_operationReturnTypes.TryGetValue(operationName, out type))
         {
            throw new OperationNotFoundException(operationName, _ownName, _info.ClassName);
         }
         object result = _server.Invoke(_originalName, operationName, arguments);
         return _typeCache.MapValue(type.ClrType, type.OpenType, result);
      }
      #endregion

      #region IMBeanRegistration Members
      public void PostDeregister()
      {
      }
      public void PostRegister(bool registrationDone)
      {
      }
      public void PreDeregister()
      {
      }
      public ObjectName PreRegister(IMBeanServer server, ObjectName name)
      {
         _server = server;
         _ownName = name;
         return name;
      }
      #endregion

      #region Utility struct
      private class OpenAndClrType
      {
         private readonly Type _clrType;
         private readonly OpenType _openType;

         public OpenAndClrType(Type clrType, OpenType openType)
         {
            _clrType = clrType;
            _openType = openType;
         }

         public Type ClrType
         {
            get { return _clrType; }
         }
         public OpenType OpenType
         {
            get { return _openType; }
         }         
      }
      #endregion
   }
}