using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262
{
   [MessageContract(IsWrapped = false)]
   public class IsInstanceOfMessage
   {
      [MessageBodyMember]
      [XmlElement("String", Namespace = Schema.ConnectorNamespace)]
      public string Value { get; set; }

      public IsInstanceOfMessage()
      {
      }

      public IsInstanceOfMessage(string value)
      {
         Value = value;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class IsInstanceOfResponseMessage
   {
      [MessageBodyMember]
      [XmlElement("Boolean", Namespace = Schema.ConnectorNamespace)]
      public bool Value { get; set; }
      
      public IsInstanceOfResponseMessage()
      {
      }

      public IsInstanceOfResponseMessage(bool value)
      {
         Value = value;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class ResourceMetaDataTypeMessage
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Schema.ConnectorNamespace)]
      public ResourceMetaDataType DynamicMBeanResourceMetaData { get; set; }

      public ResourceMetaDataTypeMessage(ResourceMetaDataType dynamicMBeanResourceMetaData)
      {
         DynamicMBeanResourceMetaData = dynamicMBeanResourceMetaData;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class EnumerateResponseMessage
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Simon.WsManagement.Schema.EnumerationNamespace)]
      public EnumerateResponse EnumerateResponse { get; set; }

      public EnumerateResponseMessage()
      {
      }

      public EnumerateResponseMessage(EnumerateResponse enumerateResponse)
      {
         EnumerateResponse = enumerateResponse;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class PullResponseMessage
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Simon.WsManagement.Schema.EnumerationNamespace)]
      public PullResponse PullResponse { get; set; }

      public PullResponseMessage()
      {
      }

      public PullResponseMessage(PullResponse pullResponse)
      {
         PullResponse = pullResponse;
      }
   }

   [MessageContract(IsWrapped = false)]
   [KnownType(typeof(GetDefaultDomainResponse))]
   [KnownType(typeof(DynamicMBeanResource))]
   [KnownType(typeof(GetDomainsResponse))]
   public class GetResponseMessage
   {
      [MessageBodyMember]
      [XmlElement(ElementName = "XmlFragment", Namespace = Simon.WsManagement.Schema.Namespace)]      
      public object Response { get; set; }
   }

   [MessageContract(IsWrapped = false)]
   public class SetAttributesResponseMessage
   {
      [MessageBodyMember]
      [XmlElement(ElementName = "XmlFragment", Namespace = Simon.WsManagement.Schema.Namespace)]
      public object Body;

      public DynamicMBeanResource Response
      {
         get { return (DynamicMBeanResource) Body; }
      }

      public SetAttributesResponseMessage()
      {         
      }

      public SetAttributesResponseMessage(DynamicMBeanResource request)
      {
         Body = request;
      }
   }

   [MessageContract(IsWrapped = false)]
   //[KnownType(typeof(DynamicMBeanResource))]
   public class SetAttributesMessage
   {
      [MessageBodyMember]
      [XmlElement(ElementName = "XmlFragment", Namespace = Simon.WsManagement.Schema.Namespace)]
      public object Body;

      [XmlIgnore]
      public DynamicMBeanResource Request
      {
         get { return (DynamicMBeanResource) Body; }
      }

      public SetAttributesMessage()
      {         
      }
      
      public SetAttributesMessage(DynamicMBeanResource request)
      {
         Body = request;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class InvokeMessage
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Schema.ConnectorNamespace)]
      public OperationRequestType ManagedResourceOperation { get; set; }

      public InvokeMessage()
      {         
      }

      public InvokeMessage(OperationRequestType managedResourceOperation)
      {
         ManagedResourceOperation = managedResourceOperation;
      }
   }

   [MessageContract(IsWrapped = false)]
   public class InvokeResponseMessage
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Schema.ConnectorNamespace)]
      public GenericValueType ManagedResourceOperationResult { get; set; }

      public InvokeResponseMessage()
      {         
      }

      public InvokeResponseMessage(GenericValueType managedResourceOperationResult)
      {
         ManagedResourceOperationResult = managedResourceOperationResult;
      }
   } 

   [Serializable]
   [XmlType(AnonymousType = true, Namespace = Schema.ConnectorNamespace)]
   [XmlRoot(Namespace = Schema.ConnectorNamespace, IsNullable = false)]
   public class GetDefaultDomainResponse
   {
      [XmlText]
      public string DomainName
      {
         get;
         set;
      }
   }

   [Serializable]
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot(Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class GetDomainsResponse
   {
      [XmlElement("Domain")]
      public string[] DomainNames
      {
         get;
         set;
      }
   }


   [Serializable]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("Values", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class TypedMultipleValueType : IDeserializable
   {
      [XmlElement("Value")]
      public GenericValueType[] Value { get; set; }

      [XmlAttribute]
      public XmlQualifiedName leafType { get; set; }

      public TypedMultipleValueType()
      {
      }

      public TypedMultipleValueType(ICollection values)
      {
         Type elementType = values.GetType().GetInterface("ICollection`1").GetGenericArguments()[0];
         leafType = JmxTypeMapping.GetJmxXmlType(elementType.AssemblyQualifiedName);
         List<GenericValueType> valueTypes = new List<GenericValueType>();
         foreach (object value in values)
         {
            valueTypes.Add(new GenericValueType(value));
         }
         Value = valueTypes.ToArray();
      }
      public object Deserialize()
      {
         Type listType = typeof(List<>).MakeGenericType(Type.GetType(JmxTypeMapping.GetCLRTypeName(leafType)));
         object results = Activator.CreateInstance(listType);
         foreach (GenericValueType valueType in Value)
         {
            listType.GetMethod("Add").Invoke(results, new[] { valueType.Deserialize() });
         }
         return results;
      }
   }

   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("Map", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class TypedMapType : IDeserializable
   {
      [XmlElement("Entry")]
      public MapTypeEntry[] Entry { get; set; }

      [XmlAttribute]
      public XmlQualifiedName keyType { get; set; }

      [XmlAttribute]
      public XmlQualifiedName valueType { get; set; }

      public TypedMapType()
      {
      }
      public TypedMapType(IDictionary value)
      {
         Type[] argumentTypes = value.GetType().GetInterface("IDictionary`2").GetGenericArguments();
         keyType = JmxTypeMapping.GetJmxXmlType(argumentTypes[0].AssemblyQualifiedName);
         valueType = JmxTypeMapping.GetJmxXmlType(argumentTypes[1].AssemblyQualifiedName);

         List<MapTypeEntry> mapTypeEntries = new List<MapTypeEntry>();
         foreach (DictionaryEntry entry in value)
         {
            mapTypeEntries.Add(new MapTypeEntry
            {
               Key = new GenericValueType(entry.Key),
               Value = new GenericValueType(entry.Value)
            });
         }
         Entry = mapTypeEntries.ToArray();
      }

      public object Deserialize()
      {
         Type dictType = typeof(Dictionary<,>).MakeGenericType(
            Type.GetType(JmxTypeMapping.GetCLRTypeName(keyType)),
            Type.GetType(JmxTypeMapping.GetCLRTypeName(valueType)));
         object results = Activator.CreateInstance(dictType);

         foreach (MapTypeEntry entry in Entry)
         {
            dictType.GetMethod("Add").Invoke(results, new[] { entry.Key.Deserialize(), entry.Value.Deserialize() });
         }
         return results;
      }
   }

   [MessageContract(IsWrapped = true, WrapperNamespace = Simon.WsManagement.Schema.EventsNamespace)]
   public class SubscribeResponse
   {

   }
}
