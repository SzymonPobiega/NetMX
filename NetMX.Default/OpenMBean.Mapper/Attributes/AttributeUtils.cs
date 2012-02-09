using System;
using System.Resources;
using System.Reflection;
using System.ComponentModel;
using NetMX.Server.OpenMBean.Mapper.Exceptions;

namespace NetMX.Server.OpenMBean.Mapper.Attributes
{
   public static class AttributeUtils
   {
      public static string GetOpenTypeDescription(MemberInfo typeElement)
      {
         Type typeToQuery;
         if (typeElement.MemberType == MemberTypes.TypeInfo || typeElement.MemberType == MemberTypes.NestedType)
         {
            typeToQuery = (Type)typeElement;
         }
         else
         {
            typeToQuery = typeElement.DeclaringType;   
         }
         if (typeToQuery.IsDefined(typeof(OpenTypeAttribute), true))
         {
            OpenTypeAttribute mappingSpec =
               (OpenTypeAttribute) typeToQuery.GetCustomAttributes(typeof (OpenTypeAttribute), true)[0];
            if (!string.IsNullOrEmpty(mappingSpec.ResourceName))
            {
               ResourceManager manager = new ResourceManager(mappingSpec.ResourceName, typeToQuery.Assembly);
               string descr = manager.GetString(typeElement.Name);
               if (descr == null)
               {
                  throw new MissingResourceItemException(typeElement.Name, mappingSpec.ResourceName,
                                                         typeToQuery.Assembly.FullName);
               }
               return descr;
            }
         }
         if (typeElement.IsDefined(typeof(DescriptionAttribute), true))
         {
            DescriptionAttribute descrAttr =
               (DescriptionAttribute) typeElement.GetCustomAttributes(typeof (DescriptionAttribute), true)[0];
            return descrAttr.Description;
         }
         return typeElement.Name;         
      }
      public static string GetOpenTypeName(MemberInfo typeElement)
      {
         if (typeElement.IsDefined(typeof(OpenTypeAttribute), true))
         {
            OpenTypeAttribute mappingSpec =
               (OpenTypeAttribute)typeElement.GetCustomAttributes(typeof(OpenTypeAttribute), true)[0];
            if (!string.IsNullOrEmpty(mappingSpec.MappedName))
            {
               return mappingSpec.MappedName;
            }            
         }         
         return typeElement.Name;
      }
   }
}