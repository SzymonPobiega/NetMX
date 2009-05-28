//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.ServiceModel;
//using System.Text;
//using System.Xml;
//using System.Xml.Serialization;

//namespace Simon.WsManagement
//{      
//   public sealed class OptimizeEnumerationType
//   {      
//   }

//   public enum EnumerationMode
//   {
//      EnumerateEPR
//   }

//   public partial class Enumerate
//   {
//      [XmlElement(Namespace = Schema.Namespace)]
//      public OptimizeEnumerationType OptimizeEnumeration { get; set; }

//      [XmlElement(Namespace = Schema.Namespace)]
//      public string EnumerationMode { get; set; }

//      public Enumerate()
//      {
//      }

//      public Enumerate(bool optimized, EnumerationMode? mode, string filterUri, object filterExpression)
//      {                
//         if (optimized)
//         {
//            OptimizeEnumeration = new OptimizeEnumerationType();
//         }
//         if (mode != null)
//         {
//            EnumerationMode = mode.ToString();
//         }
//         if (filterUri != null)
//         {
//            this.filterField = new FilterType {Dialect = filterUri};            
//         }
//      }
//   }
//   public partial class EnumerateResponse
//   {
//      [XmlArray(ElementName = "Items", Namespace = Schema.Namespace)]
//      [XmlArrayItem(ElementName = "EndpointReference", Type = typeof(EndpointAddress10), Namespace = WsAddressing.Namespace)]
//      public List<EndpointAddress10> EnumerateEPRItems { get; set; }      

//      public EnumerateResponse()
//      {         
//      }

//      public EnumerateResponse(IEnumerable<EndpointAddress> eprs)
//      {         
//         EnumerateEPRItems = new List<EndpointAddress10>(eprs.Select(x => EndpointAddress10.FromEndpointAddress(x)));
//      }

//      public IEnumerable<EndpointAddress> DeserializeAsEPRs()
//      {
//         return EnumerateEPRItems.Select(x => x.ToEndpointAddress());
//      }
//   }
//}