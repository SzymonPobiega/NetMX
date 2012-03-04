using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using NetMX.OpenMBean;
using NetMX.Relation;
using WSMan.NET;
using WSMan.NET.Addressing;

namespace NetMX.Remote.Jsr262.Structures
{
   [XmlInclude(typeof(ParameterType))]
   [XmlInclude(typeof(NamedGenericValueType))]
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("ManagedResourceOperationResult", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class GenericValueType : IDeserializable
   {
      private ItemChoiceType itemElementNameField;
      private object itemField;


      [XmlElement("ArrayDataType", typeof(ArrayDataType_Type))]
      [XmlElement("Base64Binary", typeof(byte[]), DataType = "base64Binary")]
      [XmlElement("Boolean", typeof(bool))]
      [XmlElement("Byte", typeof(sbyte))]
      [XmlElement("Character", typeof(ushort))]
      [XmlElement("CompositeDataType", typeof(CompositeDataType_Type))]
      [XmlElement("CompositeDataValue", typeof(CompositeDataValueType))]
      [XmlElement("Custom", typeof(XmlElement))]
      [XmlElement("Date", typeof(DateTime), DataType = "date")]
      [XmlElement("DateTime", typeof(DateTime))]
      [XmlElement("Decimal", typeof(decimal))]
      [XmlElement("Double", typeof(double))]
      [XmlElement("Duration", typeof(string), DataType = "duration")]
      [XmlElement("EndpointReference", typeof(EndpointReference))]
      [XmlElement("Enumeration", typeof(Enumeration))]
      [XmlElement("Fault", typeof(ManagementFaultType))]
      [XmlElement("Float", typeof(float))]
      [XmlElement("Int", typeof(int))]
      [XmlElement("Integer", typeof(string), DataType = "integer")]
      [XmlElement("List", typeof(MultipleValueType))]
      [XmlElement("TypedList", typeof(TypedMultipleValueType))]
      [XmlElement("Long", typeof(long))]
      [XmlElement("ManagedResourceRelationType", typeof(ManagedResourceRelationType))]
      [XmlElement("ManagedResourceRole", typeof(ManagedResourceRole))]
      [XmlElement("ManagedResourceRoleInfo", typeof(ManagedResourceRoleInfo))]
      [XmlElement("ManagedResourceRoleList", typeof(ManagedResourceRoleList))]
      [XmlElement("ManagedResourceRoleResult", typeof(ManagedResourceRoleResult))]
      [XmlElement("ManagedResourceRoleUnresolved", typeof(ManagedResourceRoleUnresolved))]
      [XmlElement("ManagedResourceRoleUnresolvedList", typeof(ManagedResourceRoleUnresolvedList))]
      [XmlElement("Map", typeof(MapType))]
      [XmlElement("TypedMap", typeof(TypedMapType))]
      [XmlElement("NotificationFilter", typeof(XmlElement))]
      [XmlElement("NotificationResult", typeof(NotificationResult))]
      [XmlElement("Null", typeof(NullType))]
      [XmlElement("QName", typeof(XmlQualifiedName))]
      [XmlElement("ServiceURL", typeof(string))]
      [XmlElement("Set", typeof(MultipleValueType))]
      [XmlElement("Short", typeof(short))]
      [XmlElement("SimpleDataType", typeof(OpenDataType_Type))]
      [XmlElement("String", typeof(string))]
      [XmlElement("Table", typeof(TableType))]
      [XmlElement("TabularDataType", typeof(TabularDataType_Type))]
      [XmlElement("TabularDataValue", typeof(TabularDataValueType))]
      [XmlElement("URI", typeof(string))]
      [XmlElement("URL", typeof(string))]
      [XmlElement("UUID", typeof(string))]
      [XmlElement("Vector", typeof(MultipleValueType))]
      [XmlChoiceIdentifier("ItemElementName")]
      public object Item
      {
         get { return itemField; }
         set { itemField = value; }
      }


      [XmlIgnore]
      public ItemChoiceType ItemElementName
      {
         get { return itemElementNameField; }
         set { itemElementNameField = value; }
      }

      public GenericValueType()
      {
      }

      public GenericValueType(object value)
      {
         if (value == null)
         {
            Item = new NullType();
            ItemElementName = ItemChoiceType.Null;
         }
         else
         {
            Type valueType = value.GetType();
            if (valueType == typeof(byte[]))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Base64Binary;
            }
            else if (valueType == typeof(bool))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Boolean;
            }
            else if (valueType == typeof(sbyte))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Byte;
            }
            else if (valueType == typeof(ushort))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Character;
            }
            else if (valueType == typeof(DateTime))
            {
               Item = value;
               ItemElementName = ItemChoiceType.DateTime;
            }
            else if (valueType == typeof(float))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Float;
            }
            else if (valueType == typeof(decimal))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Decimal;
            }
            else if (valueType == typeof(double))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Double;
            }
            else if (valueType == typeof(int))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Int;
            }
            else if (valueType == typeof(long))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Long;
            }
            else if (valueType == typeof(short))
            {
               Item = value;
               ItemElementName = ItemChoiceType.Short;
            }
            else if (valueType == typeof(string))
            {
               Item = value;
               ItemElementName = ItemChoiceType.String;
            }
            else if (valueType == typeof(Uri))
            {
               Item = value.ToString();
               ItemElementName = ItemChoiceType.URI;
            }
            else if (valueType == typeof(Guid))
            {
               Item = value.ToString();
               ItemElementName = ItemChoiceType.UUID;
            }
            else if (valueType == typeof(TimeSpan))
            {
               Item = value.ToString();
               ItemElementName = ItemChoiceType.Duration;
            }
            else if (valueType.GetInterface("IDictionary`2") != null)
            {
               Item = new TypedMapType((IDictionary)value);
               ItemElementName = ItemChoiceType.TypedMap;
            }   
            else if (typeof(IDictionary).IsAssignableFrom(valueType))
            {
               Item = new MapType((IDictionary)value);
               ItemElementName = ItemChoiceType.Map;
            }
            else if (valueType.GetInterface("ICollection`1") != null)
            {
               Item = new TypedMultipleValueType((ICollection)value);
               ItemElementName = ItemChoiceType.TypedList;
            }                           
            else if (typeof(ICollection).IsAssignableFrom(valueType))
            {
               Item = new MultipleValueType((ICollection)value);
               ItemElementName = ItemChoiceType.List;
            }            
            else if (valueType == typeof(XmlQualifiedName))
            {
               Item = value;
               ItemElementName = ItemChoiceType.QName;
            }            
            else if (valueType == typeof(ObjectName))
            {
               Item = ObjectNameSelector.CreateEndpointAddress((ObjectName)value);
               ItemElementName = ItemChoiceType.EndpointReference;
            }
            else if (valueType == typeof(RoleInfo))
            {
               Item = new ManagedResourceRoleInfo((RoleInfo) value);
               ItemElementName = ItemChoiceType.ManagedResourceRoleInfo;
            }
            else if (valueType == typeof(RoleResult))
            {
               Item = new ManagedResourceRoleResult((RoleResult) value);
               ItemElementName = ItemChoiceType.ManagedResourceRoleResult;
            }
            else if (typeof(ITabularData).IsAssignableFrom(valueType))
            {
               Item = new TabularDataValueType((ITabularData)value);
               ItemElementName = ItemChoiceType.TabularDataValue;
            }
            else if (typeof(ICompositeData).IsAssignableFrom(valueType))
            {
               Item = new CompositeDataValueType((ICompositeData)value);
               ItemElementName = ItemChoiceType.CompositeDataValue;
            }
            else if (typeof(OpenType).IsAssignableFrom(valueType))
            {
               Item = OpenDataType_Type.Serialize(value, out itemElementNameField);
            }            
            else throw new NotSupportedException("Not supported type in serialization: "+valueType);
         }
      }
      public object Deserialize()
      {
         if (ItemElementName == ItemChoiceType.Null)
         {
            return null;
         }
         var deserializable = Item as IDeserializable;
         if (deserializable != null)
         {
            return deserializable.Deserialize();
         }
         if (ItemElementName == ItemChoiceType.URI || ItemElementName == ItemChoiceType.URL || ItemElementName == ItemChoiceType.ServiceURL)
         {
            return new Uri((string)Item);
         }
         if (ItemElementName == ItemChoiceType.Duration)
         {
            return TimeSpan.Parse((string)Item);
         }
         return Item;
      }
   }
}