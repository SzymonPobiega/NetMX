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
   [XmlType(AnonymousType = true, Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public class QueryExprBetween
   {
      [XmlElement("Value")]
      public ValueExpr[] Value { get; set; }
   }
}