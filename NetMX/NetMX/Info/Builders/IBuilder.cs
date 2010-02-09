using System;
using System.Linq;
using System.Collections.Generic;

namespace NetMX
{
   /// <summary>
   /// Internat interface used to provide descriptor values.
   /// </summary>
   internal interface IBuilder
   {
      /// <summary>
      /// Descriptor of MBean feature being built.
      /// </summary>
      Descriptor Descriptor { get; }
   }
}