using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using NetMX.OpenMBean;
using NetMX.Relation;
using NetMX.Remote.Jsr262.Structures;
using WSMan.NET;

namespace NetMX.Remote.Jsr262
{
   public interface IDeserializable
   {
      object Deserialize();
   }

   public partial class NamedGenericValueType
   {
      public NamedGenericValueType()
      {
      }
      public NamedGenericValueType(string name, object value)
         : base(value)
      {
         this.name = name;
      }
   }

   public partial class ParameterType : GenericValueType
   {
      public ParameterType()
      {
      }
      public ParameterType(string name, object value)
         : base(value)
      {
         this.name = name;
      }
   }

   public partial class GenericValueType : IDeserializable
   {
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
         IDeserializable deserializable = Item as IDeserializable;
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

   public partial class MapType : IDeserializable
   {
      public MapType()
      {
      }
      public MapType(IDictionary value)
      {
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
         Hashtable results = new Hashtable();
         foreach (MapTypeEntry entry in Entry)
         {
            results.Add(entry.Key.Deserialize(), entry.Value.Deserialize());
         }
         return results;
      }
   }

   public partial class MultipleValueType : IDeserializable
   {
      public MultipleValueType()
      {
      }

      public MultipleValueType(ICollection values)
      {
         List<GenericValueType> valueTypes = new List<GenericValueType>();
         foreach (object value in values)
         {
            valueTypes.Add(new GenericValueType(value));
         }
         Value = valueTypes.ToArray();
      }
      public object Deserialize()
      {
         ArrayList results = new ArrayList();
         foreach (GenericValueType valueType in Value)
         {
            results.Add(valueType.Deserialize());
         }
         return results;
      }
   }   

   public partial class ManagedResourceRoleInfo : IDeserializable
   {
      public ManagedResourceRoleInfo()
      {
         
      }
      public ManagedResourceRoleInfo(RoleInfo roleInfo)
      {
         accessField = "";
         if (roleInfo.Readable)
         {
            accessField += "r";
         }
         if (roleInfo.Writable)
         {
            accessField += "w";
         }
         minDegreeField = roleInfo.MinDegree;
         maxDegreeField = roleInfo.MaxDegree;
         nameField = roleInfo.Name;
         descriptionField = roleInfo.Description;
         managedResourceClassNameField = roleInfo.RefMBeanClassName;
      }

      public object Deserialize()
      {
         return new RoleInfo(nameField, managedResourceClassNameField,
                             accessField.IndexOf('r') != -1,
                             accessField.IndexOf('w') != -1,
                             minDegreeField,
                             maxDegreeField,
                             descriptionField);
      }
   }

   public partial class ManagedResourceRole
   {
      public ManagedResourceRole()
      {         
      }
      public ManagedResourceRole(Role role)
      {
         name = role.Name;
         Value = role.Value.Select(x => new EndpointReference(ObjectNameSelector.CreateEndpointAddress(x))).ToArray();
      }
      public Role Deserialize()
      {
         return new Role(name, Value.Select(x => x.Address.ExtractObjectName()));
      }
   }

   public partial class ManagedResourceRoleUnresolved
   {
      public ManagedResourceRoleUnresolved()
      {         
      }
      public ManagedResourceRoleUnresolved(RoleUnresolved role)
      {
         name = role.RoleName;
         Value = role.RoleValue.Select(x => new EndpointReference(ObjectNameSelector.CreateEndpointAddress(x))).ToArray();
         problemSpecified = true;
         problem = (int)role.ProblemType;
      }
      public RoleUnresolved Deserialize()
      {
         return new RoleUnresolved(name, Value.Select(x => x.Address.ExtractObjectName()), (RoleStatus)problem);
      }
   }

   public partial class ManagedResourceRoleResult : IDeserializable
   {
      public ManagedResourceRoleResult()
      {         
      }
      public ManagedResourceRoleResult(RoleResult roleResult)
      {
         ManagedResourceRoleList = new ManagedResourceRoleList
                                      {
                                         ManagedResourceRole =
                                            roleResult.Roles.Select(x => new ManagedResourceRole(x)).ToArray()
                                      };
         ManagedResourceRoleUnresolvedList = new ManagedResourceRoleUnresolvedList
                                                {
                                                   ManagedResourceRoleUnresolved =
                                                      roleResult.UnresolvedRoles.Select(
                                                      x => new ManagedResourceRoleUnresolved(x)).ToArray()
                                                };
      }

      public object Deserialize()
      {
         return new RoleResult(
            ManagedResourceRoleList.ManagedResourceRole.Select(x => x.Deserialize()),
            ManagedResourceRoleUnresolvedList.ManagedResourceRoleUnresolved.Select(x => x.Deserialize()));
      }
   }       
}
