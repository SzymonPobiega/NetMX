using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Configuration;

namespace NetMX.Remote.Jsr262
{
   public class Soap12Addressing200408WSHttpBindingElement : WSHttpBindingElement
   {
      public Soap12Addressing200408WSHttpBindingElement()
         : this(null)
      {
      }

      public Soap12Addressing200408WSHttpBindingElement(string name)
         : base(name)
      {
      }

      protected override Type BindingElementType
      {
         get
         {
            return typeof(Soap12Addressing200408WSHttpBinding);
         }
      }
   }
}