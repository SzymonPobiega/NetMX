using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using NetMX.OpenMBean;
using NetMX.Relation;

namespace NetMX.Remote.Jsr262
{
   /// <summary>
   /// Maps CLR types to JSR-262 xsd equivalents and vice versa.
   /// </summary>
   public static class JmxTypeMapping
   {
      private static readonly Dictionary<string, string> _reverseMapping = new Dictionary<string, string>()
                                                                              {
                                                                                 { "List", typeof(ArrayList).AssemblyQualifiedName },
                                                                                 { "Map", typeof(Hashtable).AssemblyQualifiedName},
                                                                                 { "Vector", typeof(ArrayList).AssemblyQualifiedName}
                                                                              };
      private static readonly Dictionary<string, string> _forwardMapping = new Dictionary<string, string>
                                                                              {
                                                                                 { typeof(byte[]).AssemblyQualifiedName, "Base64Binary" },
                                                                                 { typeof(byte).AssemblyQualifiedName, "Byte" },
                                                                                 { typeof(bool).AssemblyQualifiedName, "Boolean" },
                                                                                 { typeof(char).AssemblyQualifiedName, "Character" },
                                                                                 { typeof(DateTime).AssemblyQualifiedName, "Date" },
                                                                                 { typeof(decimal).AssemblyQualifiedName, "Decimal" },
                                                                                 { typeof(double).AssemblyQualifiedName, "Double" },
                                                                                 { typeof(TimeSpan).AssemblyQualifiedName, "Duration" },
                                                                                 { typeof(float).AssemblyQualifiedName, "Float" },
                                                                                 { typeof(int).AssemblyQualifiedName, "Int" },
                                                                                 { typeof(long).AssemblyQualifiedName, "Long" },
                                                                                 { typeof(XmlQualifiedName).AssemblyQualifiedName, "QName" },
                                                                                 { typeof(short).AssemblyQualifiedName, "Short" },
                                                                                 { typeof(string).AssemblyQualifiedName, "String" },
                                                                                 { typeof(Uri).AssemblyQualifiedName, "URI" },
                                                                                 { typeof(Guid).AssemblyQualifiedName, "UUID" },                                                        
                                                                                 { typeof(Role).AssemblyQualifiedName, "ManagedResourceRole" },
                                                                                 { typeof(RelationTypeSupport).AssemblyQualifiedName, "ManagedResourceRelationType" },
                                                                                 { typeof(RoleInfo).AssemblyQualifiedName, "ManagedResourceRoleInfo" },
                                                                                 { typeof(RoleUnresolved).AssemblyQualifiedName, "ManagedResourceRoleUnresolved" },
                                                                                 { typeof(RoleResult).AssemblyQualifiedName, "ManagedResourceRoleResult" },
                                                                                 { typeof(ObjectName).AssemblyQualifiedName, "EndpointReference" }, 
                                                                                 { typeof(ITabularData).AssemblyQualifiedName, "TabularDataValue" }, 
                                                                                 { typeof(ICompositeData).AssemblyQualifiedName, "CompositeDataValue" }, 
                                                                                 { typeof(SimpleType).AssemblyQualifiedName, "SimpleDataType" }, 
                                                                                 { typeof(CompositeType).AssemblyQualifiedName, "CompositeDataType" }, 
                                                                                 { typeof(ArrayType).AssemblyQualifiedName, "ArrayDataType" }, 
                                                                                 { typeof(TabularType).AssemblyQualifiedName, "TabularDataType" }, 
                                                                                 //{ typeof().AssemblyQualifiedName, "" }, 
                                                                              };
      static JmxTypeMapping()
      {
         foreach (KeyValuePair<string, string> pair in _forwardMapping)
         {
            _reverseMapping[pair.Value] = pair.Key;
         }
      }

      /// <summary>
      /// Maps JSR-262 type representation to CLR type's assembly qualified name.
      /// </summary>
      /// <param name="jmxXmlRepresentation">JSR-262 representation.</param>
      /// <returns></returns>
      public static string GetCLRTypeName(XmlQualifiedName jmxXmlRepresentation)
      {
         if (jmxXmlRepresentation == null)
         {
            return typeof(void).AssemblyQualifiedName;
         }
         
         return GetCLRTypeName(jmxXmlRepresentation.Name);
      }

      private static string GetCLRTypeName(string jmxTypeName)
      {
         string simple;
         if (string.IsNullOrEmpty(jmxTypeName ))
         {
            return typeof (void).AssemblyQualifiedName;
         }
         if (_reverseMapping.TryGetValue(jmxTypeName, out simple))
         {            
            return simple;
         }
         if (jmxTypeName.StartsWith("ListOf"))
         {
            string elementTypeName = jmxTypeName.Remove(0, 6);            
            Type elementType = Type.GetType(GetCLRTypeName(elementTypeName));
            Type listType = typeof (IList<>).MakeGenericType(elementType);
            return listType.AssemblyQualifiedName;
         }
         if (jmxTypeName.StartsWith("MapFrom"))
         {
            string elementTypeName = jmxTypeName.Remove(0, 7);
            string[] argumentNames = elementTypeName.Split(new[] {"To"}, StringSplitOptions.RemoveEmptyEntries);
            Type keyType = Type.GetType(GetCLRTypeName(argumentNames[0]));
            Type valueType = Type.GetType(GetCLRTypeName(argumentNames[1]));
            Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(keyType, valueType);
            return dictionaryType.AssemblyQualifiedName;
         }
         throw new NotSupportedException("JMX type is not supported: "+jmxTypeName);
      }


      /// <summary>
      /// Maps CLR type name to it's JRS-262 representation. <see cref="IDictionary"/> implementations are mapped to "Map" and other 
      /// <see cref="ICollection"/> types are mapped to "List".
      /// </summary>
      /// <param name="clrTypeName">CLR assembly qualified type name.</param>
      /// <returns></returns>
      public static XmlQualifiedName GetJmxXmlType(string clrTypeName)
      {
         return new XmlQualifiedName(GetJmxXmlTypeName(clrTypeName), Schema.ConnectorNamespace);
      }

      public static string GetJmxXmlTypeName(string clrTypeName)
      {
         string simple;
         if (_forwardMapping.TryGetValue(clrTypeName, out simple))
         {
            return simple;
         }
         Type clrType = Type.GetType(clrTypeName);
         if (typeof(void) == clrType)
         {
            return null;
         }
         if (clrType.GetInterface("IDictionary`2") != null)
         {
            Type[] arguments = clrType.GetInterface("IDictionary`2").GetGenericArguments();
            return string.Format("MapFrom{0}To{1}",
                                 GetJmxXmlTypeName(arguments[0].AssemblyQualifiedName),
                                 GetJmxXmlTypeName(arguments[1].AssemblyQualifiedName));
         }
         if (clrType.GetInterface("ICollection`1") != null)
         {
            Type elementType = clrType.GetInterface("ICollection`1").GetGenericArguments()[0];
            return "ListOf" + GetJmxXmlTypeName(elementType.AssemblyQualifiedName);
         }
         if (typeof(IDictionary).IsAssignableFrom(clrType))
         {
            return "Map";
         }
         if (typeof(ICollection).IsAssignableFrom(clrType))
         {
            return "List";
         }         
         throw new NotSupportedException("JMX type is not supported: "+clrTypeName);
      }
   }
}