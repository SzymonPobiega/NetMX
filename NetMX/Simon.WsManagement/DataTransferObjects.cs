﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:2.0.50727.3082
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ServiceModel;
using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace Simon.WsManagement
{   

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   public partial class LanguageSpecificStringType
   {

      private string langField;

      private System.Xml.XmlAttribute[] anyAttrField;

      private string valueField;

      /// <uwagi/>
      [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
      public string lang
      {
         get
         {
            return this.langField;
         }
         set
         {
            this.langField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlTextAttribute()]
      public string Value
      {
         get
         {
            return this.valueField;
         }
         set
         {
            this.valueField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   public partial class EnumerationContextType
   {

      private string[] textField;

      /// <uwagi/>
      [System.Xml.Serialization.XmlTextAttribute()]
      public string[] Text
      {
         get
         {
            return this.textField;
         }
         set
         {
            this.textField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   public partial class ItemListType
   {

      private System.Xml.XmlElement[] anyField;

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   public partial class FilterType
   {

      private System.Xml.XmlNode[] anyField;

      private string dialectField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      [System.Xml.Serialization.XmlTextAttribute()]
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlNode[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
      public string Dialect
      {
         get
         {
            return this.dialectField;
         }
         set
         {
            this.dialectField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class Enumerate
   {

      private EndpointAddress10 endToField;

      private string expiresField;

      private FilterType filterField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EndpointAddress10 EndTo
      {
         get
         {
            return this.endToField;
         }
         set
         {
            this.endToField = value;
         }
      }

      /// <uwagi/>
      public string Expires
      {
         get
         {
            return this.expiresField;
         }
         set
         {
            this.expiresField = value;
         }
      }

      /// <uwagi/>
      public FilterType Filter
      {
         get
         {
            return this.filterField;
         }
         set
         {
            this.filterField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class EnumerateResponse
   {

      private string expiresField;

      private EnumerationContextType enumerationContextField;

      private System.Xml.XmlNode[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public string Expires
      {
         get
         {
            return this.expiresField;
         }
         set
         {
            this.expiresField = value;
         }
      }

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlNode[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class Pull
   {

      private EnumerationContextType enumerationContextField;

      private string maxTimeField;

      private string maxElementsField;

      private string maxCharactersField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
      public string MaxTime
      {
         get
         {
            return this.maxTimeField;
         }
         set
         {
            this.maxTimeField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
      public string MaxElements
      {
         get
         {
            return this.maxElementsField;
         }
         set
         {
            this.maxElementsField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
      public string MaxCharacters
      {
         get
         {
            return this.maxCharactersField;
         }
         set
         {
            this.maxCharactersField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class PullResponse
   {

      private EnumerationContextType enumerationContextField;

      private ItemListType itemsField;

      private object endOfSequenceField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      public ItemListType Items
      {
         get
         {
            return this.itemsField;
         }
         set
         {
            this.itemsField = value;
         }
      }

      /// <uwagi/>
      public object EndOfSequence
      {
         get
         {
            return this.endOfSequenceField;
         }
         set
         {
            this.endOfSequenceField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class Renew
   {

      private EnumerationContextType enumerationContextField;

      private string expiresField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      public string Expires
      {
         get
         {
            return this.expiresField;
         }
         set
         {
            this.expiresField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class RenewResponse
   {

      private string expiresField;

      private EnumerationContextType enumerationContextField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public string Expires
      {
         get
         {
            return this.expiresField;
         }
         set
         {
            this.expiresField = value;
         }
      }

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class GetStatus
   {

      private EnumerationContextType enumerationContextField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class GetStatusResponse
   {

      private string expiresField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public string Expires
      {
         get
         {
            return this.expiresField;
         }
         set
         {
            this.expiresField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class Release
   {

      private EnumerationContextType enumerationContextField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }

   /// <uwagi/>
   [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration")]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration", IsNullable = false)]
   public partial class EnumerationEnd
   {

      private EnumerationContextType enumerationContextField;

      private string codeField;

      private LanguageSpecificStringType[] reasonField;

      private System.Xml.XmlElement[] anyField;

      private System.Xml.XmlAttribute[] anyAttrField;

      /// <uwagi/>
      public EnumerationContextType EnumerationContext
      {
         get
         {
            return this.enumerationContextField;
         }
         set
         {
            this.enumerationContextField = value;
         }
      }

      /// <uwagi/>
      public string Code
      {
         get
         {
            return this.codeField;
         }
         set
         {
            this.codeField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlElementAttribute("Reason")]
      public LanguageSpecificStringType[] Reason
      {
         get
         {
            return this.reasonField;
         }
         set
         {
            this.reasonField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyElementAttribute()]
      public System.Xml.XmlElement[] Any
      {
         get
         {
            return this.anyField;
         }
         set
         {
            this.anyField = value;
         }
      }

      /// <uwagi/>
      [System.Xml.Serialization.XmlAnyAttributeAttribute()]
      public System.Xml.XmlAttribute[] AnyAttr
      {
         get
         {
            return this.anyAttrField;
         }
         set
         {
            this.anyAttrField = value;
         }
      }
   }
}
