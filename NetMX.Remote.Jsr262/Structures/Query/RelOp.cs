using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetMX.Remote.Jsr262.Structures.Query
{
   [GeneratedCode("xsd", "2.0.50727.1432")]
   [Serializable]
   [XmlType(Namespace = "http://jsr262.dev.java.net/jmxconnector")]
   public enum RelOp
   {
      [XmlEnum("=")]
      Item,


      [XmlEnum("!=")]
      Item1,


      [XmlEnum("<")]
      Item2,


      [XmlEnum(">")]
      Item3,


      [XmlEnum("<=")]
      Item4,


      [XmlEnum(">=")]
      Item5,
   }
}