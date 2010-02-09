using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Represents possible MBean operation impacts.
   /// </summary>
   public enum OperationImpact
   {
      /// <summary>
      /// Impact on MBean data is unknown.
      /// </summary>
      Unknown = 0,
      /// <summary>
      /// Operation is a query and does not change MBean data.
      /// </summary>
      Info = 1,
      /// <summary>
      /// Operation is a mutator and can change MBean data.
      /// </summary>
      Action = 2
   }
}