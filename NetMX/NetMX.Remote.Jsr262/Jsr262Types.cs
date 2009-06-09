using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262
{
   [Serializable]
   [XmlRoot("XmlFragment", Namespace = Simon.WsManagement.Schema.Namespace)]
   public class DynamicMBeanResourceFragment
   {
      private NamedGenericValueType[] propertyField;

      /// <remarks/>
      [XmlElement("Property")]
      public NamedGenericValueType[] Property
      {
         get { return propertyField; }
         set { propertyField = value; }
      }
   }

   [Serializable]
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot(Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class GetDefaultDomainResponse
   {      
      [XmlText]
      public string DomainName
      {
         get; set;
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
         Type listType = typeof (List<>).MakeGenericType(Type.GetType(JmxTypeMapping.GetCLRTypeName(leafType)));
         object results = Activator.CreateInstance(listType);
         foreach (GenericValueType valueType in Value)
         {
            listType.GetMethod("Add").Invoke(results, new[] {valueType.Deserialize()});            
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


}
