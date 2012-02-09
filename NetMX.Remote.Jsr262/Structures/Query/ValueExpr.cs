using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures.Query
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("Value", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class ValueExpr
   {
      private object itemField;


      [XmlElement("Attr", typeof(ValueExprAttr))]
      [XmlElement("Boolean", typeof(bool))]
      [XmlElement("Class", typeof(ValueExprClass))]
      [XmlElement("Double", typeof(double))]
      [XmlElement("Long", typeof(long))]
      [XmlElement("Op", typeof(ValueExprOP))]
      [XmlElement("String", typeof(string))]
      public object Item
      {
         get { return itemField; }
         set { itemField = value; }
      }
   }
}