using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures.Query
{
   [Serializable]

   [DesignerCategory("code")]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public class TwoQueries
   {
      [XmlElement("Query")]
      public QueryExpr[] Query { get; set; }
   }
}