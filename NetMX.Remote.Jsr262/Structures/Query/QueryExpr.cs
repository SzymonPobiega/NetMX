using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures.Query
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   [XmlRoot("Query", Namespace = "http://jsr262.dev.java.net/jmxconnector", IsNullable = false)]
   public class QueryExpr
   {
      private object itemField;


      [XmlElement("And", typeof(TwoQueries))]
      [XmlElement("Between", typeof(QueryExprBetween))]
      [XmlElement("In", typeof(QueryExprIN))]
      [XmlElement("InstanceOf", typeof(ValueExpr))]
      [XmlElement("Match", typeof(TwoValues))]
      [XmlElement("MatchName", typeof(string))]
      [XmlElement("Not", typeof(QueryExprNot))]
      [XmlElement("Or", typeof(TwoQueries))]
      [XmlElement("Rel", typeof(QueryExprRel))]
      [XmlChoiceIdentifier("ItemElementName")]
      public object Item
      {
         get { return itemField; }
         set { itemField = value; }
      }

      [XmlIgnore]
      public ItemChoiceType1 ItemElementName { get; set; }
   }
}
