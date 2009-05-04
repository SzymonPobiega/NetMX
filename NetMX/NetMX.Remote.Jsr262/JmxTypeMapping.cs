using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

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
         string simple;
         if (_reverseMapping.TryGetValue(jmxXmlRepresentation.Name, out simple))
         {
            return simple;
         }         
         throw new NotSupportedException("Type is not supported.");
      }
      
      /// <summary>
      /// Maps CLR type name to it's JRS-262 representation. <see cref="IDictionary"/> implementations are mapped to "Map" and other 
      /// <see cref="ICollection"/> types are mapped to "List".
      /// </summary>
      /// <param name="clrTypeName">CLR assembly qualified type name.</param>
      /// <returns></returns>
      public static XmlQualifiedName GetJmxXmlType(string clrTypeName)
      {
         string simple;
         if (_forwardMapping.TryGetValue(clrTypeName, out simple))
         {
            return new XmlQualifiedName(simple, Schema.ConnectorNamespace);
         }
         Type clrType = Type.GetType(clrTypeName);
         if (typeof(void) == clrType)
         {
            return null;
         }
         if (typeof(IDictionary).IsAssignableFrom(clrType))
         {
            return new XmlQualifiedName("Map", Schema.ConnectorNamespace);
         }
         if (typeof(ICollection).IsAssignableFrom(clrType))
         {
            return new XmlQualifiedName("List", Schema.ConnectorNamespace);
         }
         throw new NotSupportedException("Type is not supported.");
      }
   }
}